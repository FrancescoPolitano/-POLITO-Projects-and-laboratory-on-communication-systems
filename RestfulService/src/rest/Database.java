package rest;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.SQLIntegrityConstraintViolationException;
import java.sql.Statement;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;

import javax.mail.internet.AddressException;
import javax.mail.internet.InternetAddress;

public class Database {
	private static Database instance = null;
	private static Connection conn = null;
	private static Object lock = new Object();

	//Initialize the jdbc driver
	private Database() {
		try {
			Class.forName("com.mysql.jdbc.Driver").newInstance();
		} catch (ClassNotFoundException ex) {
			System.out.println("Error: unable to load driver class!");
		} catch (IllegalAccessException ex) {
			System.out.println("Error: access problem while loading!");
		} catch (InstantiationException ex) {
			System.out.println("Error: unable to instantiate driver!");
		}
	}

	//Create thread safe Singleton
	public static Database getInstance() {
		if (instance == null) {
			synchronized (lock) {
				if (instance == null)
					instance = new Database();
			}
		}
		return instance;
	}

	//Return the connection to the database
	public Connection connect() {
		String user = "root";
		String password = user;
		try {
			conn = DriverManager.getConnection("jdbc:mysql://localhost:3306/paldb", user, password);
		} catch (SQLException e) {
			System.out.println("Eccezione catturata" + e.getMessage());
		}
		if (conn != null)
			System.out.println("Connected to the database");
		return conn;
	}

	
	public ArrayList<Employee> getAllEmployes() {
		ArrayList<Employee> list = new ArrayList<Employee>();
		Statement stmt = null;
		Connection conn = connect();
		if (conn == null)
			return null;
		try {
			stmt = (Statement) conn.createStatement();
			stmt.executeQuery("SELECT e.*,a.Result, l.Name, a.TimeS, photo\r\n"
					+ "FROM employes e , photos p, locals l, accesses a\r\n"
					+ "WHERE p.IdEmployee= e.SerialNumber AND \r\n"
					+ "e.Causal IS NULL AND e.Expiration IS NULL AND a.IdEmployee=e.SerialNumber AND l.Id=a.IdLocal\r\n"
					+ "AND a.TimeS in (SELECT MAX(TimeS) FROM accesses where Result = 'true' GROUP BY IdEmployee)\r\n"
					+ "GROUP BY e.SerialNumber");
			ResultSet rs = stmt.getResultSet();
			while (rs.next()) {
				String serial = rs.getString("SerialNumber");
				String name = rs.getString("Name");
				String surname = rs.getString("Surname");
				String auth = rs.getString("AuthGrade");
				String position = rs.getString("l.Name");
				String photo = rs.getString("photo");
				String email = rs.getString("Email");

				Employee temp = new Employee(serial, name, surname, auth, position, email);
				temp.setPhoto(photo);
				list.add(temp);
			}
			stmt.executeQuery("SELECT e.*, photo from employes e, photos p \r\n"
					+ " where  p.IdEmployee=e.SerialNumber and e.Causal IS NULL AND e.Expiration IS NULL and e.SerialNumber not  in(SELECT e.SerialNumber from employes e , locals l, accesses a \r\n"
					+ " where a.IdEmployee=e.SerialNumber and l.Id=a.IdLocal and a.Result = 'true' GROUP BY e.SerialNumber)\r\n"
					+ " GROUP BY e.SerialNumber");
			rs = stmt.getResultSet();
			while (rs.next()) {
				String serial = rs.getString("SerialNumber");
				String name = rs.getString("Name");
				String surname = rs.getString("Surname");
				String auth = rs.getString("AuthGrade");
				String position = "Position not found";
				String photo = rs.getString("photo");
				String email = rs.getString("Email");

				Employee temp = new Employee(serial, name, surname, auth, position, email);
				temp.setPhoto(photo);
				list.add(temp);
			}
			return list;
		} catch (SQLException ex) {
		} finally {
			try {
				if (stmt != null)
					stmt.close();
			} catch (SQLException e) {
			}
		}
		return list;
	}

	public ArrayList<Visitor> getAllVisitors() {
		ArrayList<Visitor> list = new ArrayList<Visitor>();
		Statement stmt = null;
		Connection conn = connect();
		if (conn == null)
			return null;
		try {
			stmt = (Statement) conn.createStatement();
			stmt.executeQuery("SELECT e.*,a.Result, l.Name, max(a.TimeS) from employes e , locals l, accesses a \r\n"
					+ "where e.Causal IS NOT NULL AND e.Expiration IS NOT NULL and a.IdEmployee=e.SerialNumber and l.Id=a.IdLocal\r\n"
					+ "AND a.TimeS in (SELECT MAX(TimeS)  FROM accesses WHERE Result = 'true' GROUP BY IdEmployee)"
					+ "GROUP BY e.SerialNumber");

			ResultSet rs = stmt.getResultSet();
			while (rs.next()) {
				String serial = rs.getString("SerialNumber");
				String name = rs.getString("Name");
				String surname = rs.getString("Surname");
				String causal = rs.getString("Causal");
				String expiration = rs.getString("Expiration");
				String position = rs.getString("l.Name");
				String authLevel = rs.getString("AuthGrade");

				Visitor temp = new Visitor(name, surname, causal, expiration, authLevel, position);
				temp.setSerial(serial);
				list.add(temp);
			}
			stmt.executeQuery("SELECT e.* from employes e\r\n"
					+ "where e.Causal IS NOT NULL and e.SerialNumber not  in(SELECT e.SerialNumber from employes e , locals l, accesses a \r\n"
					+ "where a.IdEmployee=e.SerialNumber and l.Id=a.IdLocal and a.Result = 'true' GROUP BY e.SerialNumber)");
			rs = stmt.getResultSet();
			while (rs.next()) {
				String serial = rs.getString("SerialNumber");
				String name = rs.getString("Name");
				String surname = rs.getString("Surname");
				String causal = rs.getString("Causal");
				String expiration = rs.getString("Expiration");
				String position = "Position not found";
				String authLevel = rs.getString("AuthGrade");

				Visitor temp = new Visitor(name, surname, causal, expiration, authLevel, position);
				temp.setSerial(serial);
				list.add(temp);
			}
		} catch (SQLException ex) {
		} finally {
			try {
				if (stmt != null)
					stmt.close();
			} catch (SQLException e) {
			}
		}
		return list;
	}

	public Employee getEmployee(String id) throws SQLException {
		Employee temp = null;
		Connection conn = connect();
		PreparedStatement ps = null;
		if (conn == null)
			return null;
		try {
			String sql = "SELECT e.*, l.Name, a.TimeS , photo from employes e , locals l, accesses a, photos p "
					+ "where p.IdEmployee= e.SerialNumber and a.TimeS= (select max(TimeS) from accesses a where a.IdEmployee= ? "
					+ "and a.IdEmployee=e.SerialNumber and a.Result='true' and l.Id=a.IdLocal) ";
			ps = conn.prepareStatement(sql);
			ps.setString(1, id);
			ps.executeQuery();
			ResultSet rs = ps.getResultSet();
			while (rs.next()) {
				String serial = rs.getString("SerialNumber");
				String name = rs.getString("Name");
				String surname = rs.getString("Surname");
				String auth = rs.getString("AuthGrade");
				String position = rs.getString("l.Name");
				String photo = rs.getString("photo");
				String email = rs.getString("Email");

				temp = new Employee(serial, name, surname, auth, position, email);
				temp.setPhoto(photo);
			}
			if (temp != null)
				return temp;
			else {
				String sql1 = "SELECT e.* , photo from employes e, photos p where p.IdEmployee= e.SerialNumber and e.SerialNumber= ? "
						+ " and e.SerialNumber  not  in(SELECT a.IdEmployee from accesses a) ";
				ps.close();
				ps = conn.prepareStatement(sql1);
				ps.setInt(1, Integer.parseInt(id));
				ps.executeQuery();
				rs = ps.getResultSet();
				while (rs.next()) {
					String serial = rs.getString("SerialNumber");
					String name = rs.getString("Name");
					String surname = rs.getString("Surname");
					String auth = rs.getString("AuthGrade");
					String position = "Position not found";
					String photo = rs.getString("photo");
					String email = rs.getString("Email");

					temp = new Employee(serial, name, surname, auth, position, email);
					temp.setCurrentPosition(position);
					temp.setPhoto(photo);
					temp.setSerial(serial);
				}
			}
		} catch (SQLException ex) {
			return null;
		} catch (Exception e) {
			return null;
		} finally {
			try {
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
			}
		}
		return temp;
	}

	public boolean isAuth(String local, String code) {
		Integer serial = null;
		int authGrade = 0, requestedGrade = 0;
		Connection conn = connect();
		if (conn == null)
			return false;

		String causal = null, expiration = null;
		PreparedStatement ps = null;
		try {
			//Disable autoCommit in order to create a Transaction
			conn.setAutoCommit(false);
			String sql1 = "select SerialNumber, e.AuthGrade , l.AuthGrade, Causal, Expiration from employes e , auth a , locals l"
					+ " where a.Code = ? and a.IdEmployee= e.SerialNumber and l.Id= ?";
			ps = conn.prepareStatement(sql1);
			ps.setString(1, code);
			ps.setString(2, local);
			ps.executeQuery();
			ResultSet rs = ps.getResultSet();
			while (rs.next()) {
				serial = rs.getInt("SerialNumber");
				authGrade = rs.getInt("e.Authgrade");
				requestedGrade = rs.getInt("l.Authgrade");
				causal = rs.getString("Causal");
				expiration = rs.getString("Expiration");
			}
			if (serial == null)
				return false;
			if (causal != null) {
				Date now = new Date();
				SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
				Date expiry = sdf.parse(expiration);
				if (expiry.before(now))
					return false;
			}
			String sql = "INSERT INTO accesses (IdEmployee,IdLocal,Result) VALUES( ?, ?, ?)";
			boolean result = false;
			if (authGrade >= requestedGrade)
				result = true;
			ps.close();
			ps = conn.prepareStatement(sql);
			ps.setInt(1, serial);
			ps.setString(2, local);
			ps.setString(3, String.valueOf(result));
			ps.executeUpdate();
			conn.commit();
			return result;
		} catch (SQLException ex) {
			try {
				conn.rollback();
			} catch (SQLException e) {
				e.printStackTrace();
			}
			return false;
		} catch (ParseException e) {
			try {
				conn.rollback();
			} catch (SQLException ex) {
				ex.printStackTrace();
			}
			return false;
		} finally {
			try {
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
			}
		}
	}

	public boolean deleteEmployee(String id) {
		PreparedStatement ps = null;
		Connection conn = connect();
		if (conn == null)
			return false;
		try {
			String sql = "DELETE FROM employes WHERE SerialNumber= ? ";
			ps = conn.prepareStatement(sql);
			ps.setString(1, id);
			ps.executeUpdate();
			return true;
		} catch (SQLException ex) {
			return false;
		} finally {
			try {
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
			}
		}
	}

	public String newCode(String id) {
		String image = null;
		Connection conn = connect();
		if (conn == null)
			return null;
		PreparedStatement ps = null;
		try {
			conn.setAutoCommit(false);
			String sql1 = "SELECT COUNT(*) as total FROM Employes WHERE SerialNumber= ?";
			ps = conn.prepareStatement(sql1);
			ps.setString(1, id);
			ps.executeQuery();
			ResultSet rs, rsCode, rsEmployee;
			String newCode, email;

			rsEmployee = ps.getResultSet();
			rsEmployee.first();
			if (rsEmployee.getInt("total") != 1)
				return null;
			String sql = "SELECT Email FROM Employes WHERE SerialNumber = ?";
			ps.close();
			ps = conn.prepareStatement(sql);
			ps.setString(1, id);
			ResultSet set = ps.executeQuery();
			set.first();
			email = set.getString("Email");
			try {
				InternetAddress emailAddr = new InternetAddress(email);
				emailAddr.validate();
			} catch (AddressException ex) {
				return null;
			}
			do {
				newCode = Utils.randomCodeGen();
				String sql2 = "SELECT COUNT(*) as total FROM auth WHERE Code= ? ";
				ps.close();
				ps = conn.prepareStatement(sql2);
				ps.setString(1, newCode);
				ps.executeQuery();
				rsCode = ps.getResultSet();
				rsCode.first();
			} while (rsCode.getInt("total") == 1);
			ps.close();
			ps = conn.prepareStatement("SELECT COUNT(*) as total FROM auth WHERE IdEmployee= ?");
			ps.setString(1, id);
			ps.executeQuery();
			rs = ps.getResultSet();
			rs.first();
			int total = rs.getInt("total");
			ps.close();
			if (total == 0) {
				ps = conn.prepareStatement("INSERT INTO auth (IdEmployee, Code) VALUES( ? ,? )");
				ps.setString(1, id);
				ps.setString(2, newCode);
				ps.executeUpdate();
			} else {
				ps = conn.prepareStatement("UPDATE auth SET Code= ? WHERE IdEmployee= ? ");
				ps.setString(1, newCode);
				ps.setString(2, id);
				ps.executeUpdate();
			}
			image = Utils.writeQRCode(newCode, id);
			if (this.isConfirmedEmail(id)) {
				if (Utils.sendEmail(email, id) == -1) {
					conn.rollback();
					return null;
				}
			} else
				Utils.sendConfirmationEmail(email, id);
			conn.commit();

		} catch (SQLException ex) {
			try {
				conn.rollback();
			} catch (SQLException e) {
				e.printStackTrace();
			}
		} finally {
			try {
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
			}
		}
		return image;
	}

	public ArrayList<Local> getAllLocals() {
		Statement stmt = null;
		Connection conn = connect();
		if (conn == null)
			return null;
		ArrayList<Local> locals = new ArrayList<Local>();
		try {
			stmt = (Statement) conn.createStatement();
			stmt.executeQuery("SELECT * from locals");
			ResultSet rs = stmt.getResultSet();
			while (rs.next()) {
				String id = rs.getString("Id");
				String authGrade = rs.getString("AuthGrade");
				String name = rs.getString("Name");
				locals.add(new Local(id, name, authGrade));
			}
		} catch (SQLException ex) {
			return null;
		} finally {
			try {
				if (stmt != null)
					stmt.close();
			} catch (SQLException e) {
			}
		}
		return locals;
	}

	public VisitorResponseClass createVisitor(Visitor visitor) {
		VisitorResponseClass vr = new VisitorResponseClass(visitor, null);
		vr.getVisitor().setCurrentPosition("Position not found");
		Integer visitorId = null;
		PreparedStatement ps = null;
		String serial = null;
		Connection conn = connect();
		if (conn == null)
			return null;
		try {
			conn.setAutoCommit(false);
			ps = conn.prepareStatement(
					"INSERT into employes (Name, Surname, Causal, Expiration,AuthGrade) values (?,?,?,?,?)",
					Statement.RETURN_GENERATED_KEYS);
			ps.setString(1, visitor.getName());
			ps.setString(2, visitor.getSurname());
			ps.setString(3, visitor.getCausal());
			ps.setString(4, visitor.getExpiration());
			ps.setString(5, visitor.getAuthLevel());
			ps.executeUpdate();
			try (ResultSet generatedKeys = ps.getGeneratedKeys()) {
				if (generatedKeys.next()) {
					visitorId = generatedKeys.getInt(1);
					serial = String.valueOf(visitorId);
					vr.getVisitor().setSerial(serial);
					ResultSet rs;
					String code;
					do {
						code = Utils.randomCodeGen();
						ps = conn.prepareStatement("SELECT COUNT(*) as total FROM auth WHERE Code= ?");
						ps.setString(1, code);
						ps.executeQuery();
						rs = ps.getResultSet();
						rs.first();
					} while (rs.getInt("total") == 1);
					ps.close();
					ps = conn.prepareStatement("INSERT into auth (IdEmployee, Code) values (?,?)");
					ps.setInt(1, visitorId);
					ps.setString(2, code);
					ps.executeUpdate();
					vr.setQrcodeURL(Utils.writeQRCode(code, serial));
				}
			}
			conn.commit();
		} catch (SQLException ex) {
			try {
				conn.rollback();
			} catch (SQLException e) {
				e.printStackTrace();
			}
			return null;
		} finally {
			try {
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
			}
		}
		return vr;
	}

	public int createNewLocal(Local local) {
		PreparedStatement ps = null;
		Connection conn = connect();
		if (conn == null)
			return -2;
		try {
			ps = conn.prepareStatement("INSERT into locals (Id, AuthGrade, Name) values (?,?,?)");
			ps.setString(1, local.getIdLocal());
			ps.setString(2, local.getAuthGrade());
			ps.setString(3, local.getName());
			ps.executeUpdate();
		} catch (SQLIntegrityConstraintViolationException ex) {
			ex.printStackTrace();
			return -1;
		} catch (SQLException ex) {
			return -2;
		} finally {
			try {
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
			}
		}
		return 0;
	}

	public EmployeeResponseClass createEmployee(EmployeeRequestClass temp) {
		try {
			InternetAddress emailAddr = new InternetAddress(temp.getEmployee().getEmail());
			emailAddr.validate();
		} catch (AddressException ex) {
			return null;
		}
		temp.getEmployee().setCurrentPosition("Position not found");
		String employeeId = null;
		PreparedStatement ps = null;
		String code = null, qrCode;
		Connection conn = connect();
		if (conn == null)
			return null;
		try {
			conn.setAutoCommit(false);
			ps = conn.prepareStatement("INSERT into employes (Name, Surname, AuthGrade,Email) values (?,?,?,?)",
					Statement.RETURN_GENERATED_KEYS);
			ps.setString(1, temp.getEmployee().getName());
			ps.setString(2, temp.getEmployee().getSurname());
			ps.setString(3, temp.getEmployee().getAuthLevel());
			ps.setString(4, temp.getEmployee().getEmail());
			ps.executeUpdate();

			try (ResultSet generatedKeys = ps.getGeneratedKeys()) {
				if (generatedKeys.next()) {
					employeeId = generatedKeys.getString(1);
					temp.getEmployee().setPhoto(Utils.StoreEmployeePhoto(temp.getPhoto(), employeeId));

					ResultSet rs;
					ps.close();
					do {
						code = Utils.randomCodeGen();
						ps = conn.prepareStatement("SELECT COUNT(*) as total FROM auth WHERE Code= ? ");
						ps.setString(1, code);
						ps.executeQuery();
						rs = ps.getResultSet();
						rs.first();
					} while (rs.getInt("total") == 1);
					ps.close();
					ps = conn.prepareStatement("INSERT into auth (IdEmployee, Code) values (?,?)");
					ps.setString(1, employeeId);
					ps.setString(2, code);
					ps.executeUpdate();
					ps.close();

					ps = conn.prepareStatement("INSERT into photos (IdEmployee, Photo) values (?,?)");
					ps.setString(1, employeeId);
					ps.setString(2, temp.getEmployee().getPhoto());
					ps.executeUpdate();
				}
			}
			if (employeeId == null)
				return null;

			temp.getEmployee().setSerial(employeeId);
			qrCode = Utils.writeQRCode(code, employeeId);
			if (Utils.sendConfirmationEmail(temp.getEmployee().getEmail(), employeeId) == -1) {
				conn.rollback();
				return null;
			}

			conn.commit();
		} catch (SQLException ex) {
			try {
				conn.rollback();
			} catch (SQLException e) {
				e.printStackTrace();
			}
			return null;
		} finally {
			try {
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
			}
		}
		return new EmployeeResponseClass(temp.getEmployee(), qrCode);
	}

	public boolean modifyEmployee(Employee newEmployee) {
		try {
			InternetAddress emailAddr = new InternetAddress(newEmployee.getEmail());
			emailAddr.validate();
		} catch (AddressException ex) {
			return false;
		}
		Connection conn = connect();
		if (conn == null)
			return false;
		PreparedStatement ps = null;
		try {
			String sql = "UPDATE Employes SET Name = ? , Surname = ? , AuthGrade = ?, Email = ? WHERE SerialNumber = ?";
			ps = conn.prepareStatement(sql);
			ps.setString(1, newEmployee.getName());
			ps.setString(2, newEmployee.getSurname());
			ps.setString(3, newEmployee.getAuthLevel());
			ps.setString(4, newEmployee.getEmail());
			ps.setString(5, newEmployee.getSerial());
			int affected = ps.executeUpdate();
			if (affected != 1)
				return false;
		} catch (SQLException e) {
			e.printStackTrace();
			return false;
		} finally {
			try {
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
			}
		}
		return true;
	}

	public ArrayList<Access> makeQuery(ComplexQuery query) {
		Statement stmt = null;
		Connection conn = connect();
		if (conn == null)
			return null;
		ResultSet results;
		ArrayList<Access> accessResults = new ArrayList<Access>();
		try {
			stmt = (Statement) conn.createStatement();
			String theQuery= query.toValidSQLQuery().concat(" ORDER BY a.TimeS desc");
			System.out.println(theQuery);
			stmt.executeQuery(theQuery);
			results = stmt.getResultSet();

			Access temp = null;
			while (results.next()) {
				temp = new Access();
				temp.setEmployeeId(results.getString("e.SerialNumber"));
				temp.setEmployeeName(results.getString("e.Name"));
				temp.setEmployeeSurname(results.getString("e.Surname"));
				temp.setLocalName(results.getString("l.Name"));
				temp.setTime(results.getTimestamp("a.TimeS"));
				temp.setResult(results.getBoolean("a.Result"));
				accessResults.add(temp);
			}
		} catch (SQLException ex) {
			return null;
		} finally {
			try {
				if (stmt != null)
					stmt.close();
			} catch (SQLException e) {
			}
		}
		return accessResults;
	}

	public String login(LoginData lg, String ExpiryDate) {
		PreparedStatement ps = null;
		String temp = null;
		ResultSet results, exists;
		Connection conn = connect();
		if (conn == null)
			return null;
		String hash = Utils.hashString(lg.getPassword());
		try {
			conn.setAutoCommit(false);
			ps = conn.prepareStatement("SELECT COUNT(*) as total FROM users WHERE Username= ? and Password= ? ");
			ps.setString(1, lg.getUsername());
			ps.setString(2, hash);
			ps.executeQuery();
			results = ps.getResultSet();
			results.first();
			int total = results.getInt("total");
			ps.close();
			if (total == 1) {
				do {
					temp = Utils.createToken(lg.getUsername());
					ps = conn.prepareStatement("SELECT COUNT(*) as total FROM tokens WHERE Token= ?");
					ps.setString(1, temp);
					ps.executeQuery();
					exists = ps.getResultSet();
					exists.first();
				} while (exists.getInt("total") == 1);
				ps.close();
				ps = conn.prepareStatement("INSERT into tokens (Token, User, Expiration) values ( ?,?,?)");
				ps.setString(1, temp);
				ps.setString(2, lg.getUsername());
				ps.setString(3, ExpiryDate);
				ps.executeUpdate();
			}
			conn.commit();
		} catch (SQLException ex) {
			try {
				conn.rollback();
			} catch (SQLException e) {
				e.printStackTrace();
			}
			return null;
		} finally {
			try {
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
			}
		}
		return temp;
	}

	public boolean logout(String token) {
		Connection conn = connect();
		if (conn == null)
			return false;
		PreparedStatement ps = null;
		try {
			String sql = "DELETE FROM tokens WHERE Token = ?";
			ps = conn.prepareStatement(sql);
			ps.setString(1, token);
			ps.executeUpdate();
		} catch (SQLException ex) {
			return false;
		} finally {
			try {
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
			}
		}
		return true;
	}

	public int blockUsers(int serialNumber) {
		Connection conn = connect();
		if (conn == null)
			return -1;
		ResultSet results = null;
		int affectedRows = 0;
		PreparedStatement ps = null, ps1 = null;
		String searchEmployee = "SELECT COUNT(*) AS total FROM employes WHERE SerialNumber = ?";
		try {
			conn.setAutoCommit(false);
			ps = conn.prepareStatement(searchEmployee);
			ps.setInt(1, serialNumber);
			results = ps.executeQuery();

			while (results.next())
				if (results.getInt("total") == 0)
					return 0;

			String sql = "UPDATE employes SET AuthGrade = ? WHERE SerialNumber = ?";
			ps1 = conn.prepareStatement(sql);
			ps1.setString(1, String.valueOf(0));
			ps1.setInt(2, serialNumber);
			affectedRows = ps1.executeUpdate();
			conn.commit();
		} catch (SQLException e) {
			try {
				conn.rollback();
			} catch (SQLException ex) {
				ex.printStackTrace();
			}
			e.printStackTrace();
		} finally {
			try {
				if (ps != null)
					ps.close();
				if (ps1 != null)
					ps1.close();
			} catch (SQLException e) {
			}
		}
		return affectedRows;
	}

	public boolean deleteOldTokens() {
		Connection conn = connect();
		if (conn == null)
			return false;
		PreparedStatement ps = null;
		Date now = new Date();
		SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
		String time = sdf.format(now);
		String sql = "DELETE FROM tokens WHERE Expiration < ?";
		try {
			ps = conn.prepareStatement(sql);
			ps.setString(1, time);
			ps.executeUpdate();
		} catch (SQLException e) {
			return false;
		} finally {
			try {
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
			}
		}
		return true;
	}

	public boolean isValidToken(String token) {
		if (token == null || token.isEmpty())
			return false;
		PreparedStatement ps = null;
		ResultSet results;
		Connection conn = connect();
		if (conn == null)
			return false;
		String time = null;
		Date now = new Date();
		SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
		time = sdf.format(now);
		try {
			ps = conn.prepareStatement("SELECT COUNT(*) as total FROM tokens WHERE Token= ? AND Expiration >= ?");
			ps.setString(1, token);
			ps.setString(2, time);
			ps.executeQuery();
			results = ps.getResultSet();
			results.first();
			if (results.getInt("total") == 1)
				return true;
		} catch (SQLException e) {
			return false;
		} finally {
			try {
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
			}
		}
		return false;
	}

	// returns true if the mail has been confirmed
	private boolean isConfirmedEmail(String employeeId) {
		if (employeeId == null || employeeId.isEmpty())
			return false;
		PreparedStatement ps = null;
		ResultSet results;
		Connection conn = connect();
		if (conn == null)
			return false;
		try {
			ps = conn
					.prepareStatement("SELECT COUNT(*) as total FROM employes WHERE SerialNumber= ? AND Confirmed = ?");
			ps.setString(1, employeeId);
			ps.setBoolean(2, true);
			ps.executeQuery();
			results = ps.getResultSet();
			results.first();
			if (results.getInt("total") == 1)
				return true;
		} catch (SQLException e) {
			return false;
		} finally {
			try {
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
			}
		}
		return false;
	}

	public boolean confirmEmail(String employeeId) {
		if (employeeId == null || employeeId.isEmpty())
			return false;

		PreparedStatement ps = null, ps1 = null;
		ResultSet results;
		Connection conn = connect();
		if (conn == null)
			return false;
		try {
			conn.setAutoCommit(false);
			ps = conn.prepareStatement("UPDATE employes SET Confirmed = 'true' where SerialNumber=?");
			ps.setString(1, employeeId);
			int rows = ps.executeUpdate();
			if (rows > 0) {
				ps1 = conn.prepareStatement("SELECT Email from employes WHERE SerialNumber= ?");
				ps1.setString(1, employeeId);
				ps1.executeQuery();
				results = ps1.getResultSet();
				results.first();
				String sendTo = results.getString("Email");
				if (Utils.sendEmail(sendTo, employeeId) == -1) {
					conn.rollback();
					return false;
				}
				conn.commit();
				return true;
			}
			conn.commit();
		} catch (SQLException e) {
			try {
				conn.rollback();
			} catch (SQLException ex) {
				ex.printStackTrace();
			}
			return false;
		} finally {
			try {
				if (ps != null)
					ps.close();
				if (ps1 != null)
					ps1.close();
			} catch (SQLException e) {
			}
		}
		return false;
	}
}