package rest;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.SQLIntegrityConstraintViolationException;
import java.sql.Statement;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;

public class Database {
	private static Database instance = null;
	private static Connection conn = null;

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

	public static Database getInstance() {
		if (instance == null)
			instance = new Database();
		return instance;
	}

	public Connection connect() {
		String user = "root";
		String password = user;
		if (conn != null)
			return conn;
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
			stmt.executeQuery(
					"SELECT e.*, l.Name, a.TimeS, photo\r\n" + 
					"FROM employes e , photos p, locals l, accesses a\r\n" + 
					"WHERE p.IdEmployee= e.SerialNumber AND\r\n" + 
					"e.Causal IS NULL AND a.IdEmployee=e.SerialNumber AND l.Id=a.IdLocal\r\n" + 
					"AND a.TimeS in (SELECT MAX(TimeS)\r\n" + 
					"FROM accesses\r\n" + 
					"GROUP BY IdEmployee)\r\n" + 
					"GROUP BY e.SerialNumber");
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
					+ " where  p.IdEmployee=e.SerialNumber and e.Causal IS NULL and e.SerialNumber not  in(SELECT e.SerialNumber from employes e , locals l, accesses a \r\n"
					+ " where a.IdEmployee=e.SerialNumber and l.Id=a.IdLocal GROUP BY e.SerialNumber)\r\n"
					+ " GROUP BY e.SerialNumber");
			rs = stmt.getResultSet();
			while (rs.next()) {
				String serial = rs.getString("SerialNumber");
				String name = rs.getString("Name");
				String surname = rs.getString("Surname");
				String auth = rs.getString("AuthGrade");
				String position = "No position found";
				String photo = rs.getString("photo");
				String email = rs.getString("Email");

				Employee temp = new Employee(serial, name, surname, auth, position, email);
				temp.setPhoto(photo);
				list.add(temp);
			}
			return list;
		} catch (SQLException ex) {
			System.out.println("111 " + ex.getMessage());
		} finally {
			try {
				if (stmt != null)
					stmt.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
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
			stmt.executeQuery("SELECT e.*, l.Name, max(a.TimeS) from employes e , locals l, accesses a \r\n"
					+ "where e.Causal IS NOT NULL and a.IdEmployee=e.SerialNumber and l.Id=a.IdLocal\r\n"
					+ "GROUP BY e.SerialNumber");
			ResultSet rs = stmt.getResultSet();
			while (rs.next()) {
				String serial = rs.getString("SerialNumber");
				String name = rs.getString("Name");
				String surname = rs.getString("Surname");
				String causal = rs.getString("Causal");
				String expiration = rs.getString("Expiration");
				String position = rs.getString("l.Name");

				Visitor temp = new Visitor(name, surname, causal, expiration);
				temp.setPosition(position);
				temp.setId(serial);
				list.add(temp);
			}
			stmt.executeQuery("SELECT e.* from employes e\r\n"
					+ "where e.Causal IS NOT NULL and e.SerialNumber not  in(SELECT e.SerialNumber from employes e , locals l, accesses a \r\n"
					+ "where a.IdEmployee=e.SerialNumber and l.Id=a.IdLocal GROUP BY e.SerialNumber)");
			rs = stmt.getResultSet();
			while (rs.next()) {
				String serial = rs.getString("SerialNumber");
				String name = rs.getString("Name");
				String surname = rs.getString("Surname");
				String causal = rs.getString("Causal");
				String expiration = rs.getString("Expiration");
				String position = "No position found";
				Visitor temp = new Visitor(name, surname, causal, expiration);
				temp.setPosition(position);
				temp.setId(serial);
				list.add(temp);

			}
		} catch (SQLException ex) {
			System.out.println("222 " + ex.getMessage());
		} finally {
			try {
				if (stmt != null)
					stmt.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
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
					String position = "position not found";
					String photo = rs.getString("photo");
					String email = rs.getString("Email");

					temp = new Employee(serial, name, surname, auth, position, email);
					temp.setCurrentPosition(position);
					temp.setPhoto(photo);
					temp.setSerial(serial);
				}
			}
		} catch (SQLException ex) {
			System.out.println("333 " + ex.getMessage());
		} finally {
			try {
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
			}
		}
		return temp;
	}

	public boolean isAuth(String local, String code) {
		Integer serial = null;
		Connection conn = connect();
		if (conn == null)
			return false;
		int authGrade = 0, requestedGrade = 0;
		PreparedStatement ps = null;
		try {
			String sql1 = "select SerialNumber, e.AuthGrade , l.AuthGrade from employes e , auth a , locals l"
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
			}
			if (serial == null)
				return false;
			String sql = "INSERT INTO accesses (IdEmployee,IdLocal,Result) VALUES( ?, ?, ?)";
			boolean result = false;
			if (authGrade > requestedGrade)
				result = true;
			ps.close();
			ps = conn.prepareStatement(sql);
			ps.setInt(1, serial);
			ps.setString(2, local);
			ps.setString(3, String.valueOf(result));
			ps.executeUpdate();
			return result;
		} catch (SQLException ex) {
			System.out.println("444 " + ex.getMessage());
			return false;
		} finally {
			try {
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
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
			System.out.println("555 " + ex.getMessage());
			return false;
		} finally {
			try {
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
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
			if (Utils.sendEmail(email, id) == -1) {
				conn.rollback();
				return null;
			}
			conn.commit();

		} catch (SQLException ex) {
			System.out.println("666 " + ex.getMessage());
		} finally {
			try {
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
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
			System.out.println("777 " + ex.getMessage());
			return null;
		} finally {
			try {
				if (stmt != null)
					stmt.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
			}
		}
		return locals;
	}

	public String createVisitor(Visitor visitor) {
		Integer visitorId = null;
		PreparedStatement ps = null;
		Connection conn = connect();
		if (conn == null)
			return null;
		try {
			ps = conn.prepareStatement("INSERT into employes (Name, Surname, Causal, Expiration) values (?,?,?,?)",
					Statement.RETURN_GENERATED_KEYS);
			ps.setString(1, visitor.getName());
			ps.setString(2, visitor.getSurname());
			ps.setString(3, visitor.getCausal());
			ps.setString(4, visitor.getExpiration());
			ps.executeUpdate();

			try (ResultSet generatedKeys = ps.getGeneratedKeys()) {
				if (generatedKeys.next()) {
					visitorId = generatedKeys.getInt(1);
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
					String result = Utils.writeQRCode(code, String.valueOf(visitorId));
					return result;
				}
			}
		} catch (SQLException ex) {
			System.out.println("888" + ex.getMessage());
		} finally {
			try {
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
			}
		}
		return null;
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
			System.out.println("999 " + ex.getMessage());
			return -2;
		} finally {
			try {
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
			}
		}
		return 0;
	}

	public EmployeeResponseClass createEmployee(EmployeeRequestClass temp) {
		temp.getEmployee().setCurrentPosition("position not found");
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

			if (Utils.sendEmail(temp.getEmployee().getEmail(), employeeId) == -1) {
				conn.rollback();
				return null;
			}
			qrCode = Utils.writeQRCode(code, employeeId);
			conn.commit();

		} catch (SQLException ex) {
			System.out.println("101010 " + ex.getMessage());
			return null;
		} finally {
			try {
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
			}
		}
		return new EmployeeResponseClass(temp.getEmployee(), qrCode);
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
			System.out.println("QUERYYYYY " + query.toValidSQLQuery());
			stmt.executeQuery(query.toValidSQLQuery());
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
			System.out.println("111111 " + ex.getMessage());
			return null;
		} finally {
			try {
				if (stmt != null)
					stmt.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
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
		} catch (SQLException ex) {
			System.out.println("999 " + ex.getMessage());
			return null;
		} finally {
			try {
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
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
			System.out.println("LOGOUT " + ex.getMessage());
			return false;
		} finally {
			try {
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
			}
		}
		return true;
	}

	public int changeAuthLevel(AuthLevel al) {
		Connection conn = connect();
		ResultSet results;
		PreparedStatement ps = null, ps1 = null;
		int affectedRows = 0;
		if (conn == null)
			return -1;
		String sqlSearchUsers = "SELECT COUNT(*) as total from employes WHERE SerialNumber = ?";

		try {
			ps = conn.prepareStatement(sqlSearchUsers);
			ps.setInt(1, al.getSerialNumber());
			results = ps.executeQuery();

			while (results.next())
				if (results.getInt("total") == 0)
					return 0;

			String sql = "UPDATE employes SET AuthGrade = ? WHERE SerialNumber = ?";
			ps1 = conn.prepareStatement(sql);
			ps1.setString(1, al.getAuthLevel());
			ps1.setInt(2, al.getSerialNumber());
			affectedRows = ps1.executeUpdate();

		} catch (SQLException e) {
			e.printStackTrace();
			return -1;
		} finally {
			try {
				if (ps != null)
					ps.close();
				if (ps1 != null)
					ps1.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
			}
		}
		return affectedRows;
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
		} catch (SQLException e) {
			System.out.println("BLOCK USER EXCEPTION");
			e.printStackTrace();
		} finally {
			try {
				if (ps != null)
					ps.close();
				if (ps1 != null)
					ps1.close();
			} catch (SQLException e) {
				System.out.println("Error closing");
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
				System.out.println("Error closing " + e.getMessage());
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
				System.out.println("Error closing " + e.getMessage());
			}
		}
		return false;
	}
}