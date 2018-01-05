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
import java.util.Calendar;
import java.util.Date;

public class Database {
	private static Database instance = null;

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

	private Connection connect() {
		String user = "root";
		String password = user;
		Connection conn = null;
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
		try {
			if (conn != null) {
				stmt = (Statement) conn.createStatement();
				stmt.executeQuery(
						"SELECT e.*, l.Name, max(a.TimeS), photo from employes e , photos p, locals l, accesses a  where p.IdEmployee= e.SerialNumber AND e.Causal IS NULL and a.IdEmployee=e.SerialNumber and l.Id=a.IdLocal GROUP BY e.SerialNumber");
				ResultSet rs = stmt.getResultSet();
				while (rs.next()) {
					String serial = rs.getString("SerialNumber");
					String name = rs.getString("Name");
					String surname = rs.getString("Surname");
					String auth = rs.getString("AuthGrade");
					String position = rs.getString("l.Name");
					String photo = rs.getString("photo");

					Employee temp = new Employee(serial, name, surname, auth, position);
					temp.setPhoto(photo);
					list.add(temp);
				}
				stmt.executeQuery("SELECT e.*, photo from employes e, photos p \r\n"
						+ "                		                       where  p.IdEmployee=e.SerialNumber and e.Causal IS NULL and e.SerialNumber not  in(SELECT e.SerialNumber from employes e , locals l, accesses a \r\n"
						+ "                		                                          where a.IdEmployee=e.SerialNumber and l.Id=a.IdLocal GROUP BY e.SerialNumber)\r\n"
						+ "                		                     GROUP BY e.SerialNumber");
				rs = stmt.getResultSet();
				while (rs.next()) {
					String serial = rs.getString("SerialNumber");
					String name = rs.getString("Name");
					String surname = rs.getString("Surname");
					String auth = rs.getString("AuthGrade");
					String position = "No position found";
					String photo = rs.getString("photo");

					Employee temp = new Employee(serial, name, surname, auth, position);
					temp.setPhoto(photo);
					list.add(temp);

				}
				return list;
			}
		} catch (SQLException ex) {
			System.out.println("111 " + ex.getMessage());

		} finally {
			try {
				if (conn != null)
					conn.close();
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
		try {
			if (conn != null) {
				stmt = (Statement) conn.createStatement();
				stmt.executeQuery("SELECT e.*, l.Name, max(a.TimeS)\r\n" + "from employes e , locals l, accesses a \r\n"
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
						+ "						where a.IdEmployee=e.SerialNumber and l.Id=a.IdLocal GROUP BY e.SerialNumber)\r\n"
						+ "");
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
			}
		} catch (SQLException ex) {
			System.out.println("222 " + ex.getMessage());
		} finally {
			try {
				if (conn != null)
					conn.close();
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
		Statement stmt = null;
		Connection conn = connect();
		try {
			if (conn != null) {
				stmt = (Statement) conn.createStatement();
				stmt.executeQuery(
						"SELECT e.*, l.Name, a.TimeS , photo from employes e , locals l, accesses a, photos p \r\n"
								+ "where p.IdEmployee= e.SerialNumber and a.TimeS= (select max(TimeS) from accesses a where a.IdEmployee='"
								+ id + "') \r\n"
								+ "and a.IdEmployee=e.SerialNumber and a.Result='true' and l.Id=a.IdLocal\r\n" + "");
				ResultSet rs = stmt.getResultSet();
				while (rs.next()) {
					String serial = rs.getString("SerialNumber");
					String name = rs.getString("Name");
					String surname = rs.getString("Surname");
					String auth = rs.getString("AuthGrade");
					String position = rs.getString("l.Name");
					String photo = rs.getString("photo");

					temp = new Employee(serial, name, surname, auth, position);
					temp.setPhoto(photo);

				}
				if (temp != null)
					return temp;
				else {
					stmt.executeQuery(
							"SELECT e.* , photo from employes e, photos p where p.IdEmployee= e.SerialNumber and e.SerialNumber='"
									+ id + "' and e.SerialNumber  not  in(SELECT a.IdEmployee from accesses a) ");
					rs = stmt.getResultSet();
					while (rs.next()) {
						String serial = rs.getString("SerialNumber");
						String name = rs.getString("Name");
						String surname = rs.getString("Surname");
						String auth = rs.getString("AuthGrade");
						String position = "position not found";
						String photo = rs.getString("photo");

						temp = new Employee(serial, name, surname, auth, position);
						temp.setCurrentPosition(position);
						temp.setPhoto(photo);

						temp.setSerial(serial);

					}
				}
			}
		} catch (SQLException ex) {
			System.out.println("333 " + ex.getMessage());
		} finally {
			try {
				if (conn != null)
					conn.close();
				if (stmt != null)
					stmt.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
			}
		}
		return temp;
	}

	public boolean isAuth(String local, String code) {
		Integer serial = null;
		Statement stmt = null;
		Connection conn = connect();
		int authGrade = 0, requestedGrade = 0;
		try {
			if (conn != null) {
				stmt = (Statement) conn.createStatement();
				stmt.executeQuery("select  SerialNumber, e.AuthGrade , l.AuthGrade "
						+ "from employes e , auth a , locals l" + " where a.Code ='" + code
						+ "' and a.IdEmployee= e.SerialNumber and l.Id='" + local + "'");
				ResultSet rs = stmt.getResultSet();
				while (rs.next()) {
					serial = rs.getInt("SerialNumber");
					authGrade = rs.getInt("e.Authgrade");
					requestedGrade = rs.getInt("l.Authgrade");
				}
				if (serial == null)
					return false;

				if (authGrade < requestedGrade) {
					stmt.executeUpdate("INSERT INTO accesses (IdEmployee,IdLocal,Result) VALUES ('" + serial + "','"
							+ local + "', 'false')");

					return false;
				} else {
					stmt.executeUpdate("INSERT INTO accesses (IdEmployee,IdLocal,Result) VALUES ('" + serial + "','"
							+ local + "', 'true')");
					return true;
				}
			}
		} catch (SQLException ex) {
			System.out.println("444 " + ex.getMessage());
		} finally {
			try {
				if (conn != null)
					conn.close();
				if (stmt != null)
					stmt.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
			}
		}
		return false;
	}

	public boolean deleteEmployee(String id) {
		Statement stmt = null;
		Connection conn = connect();
		try {
			if (conn != null) {
				stmt = (Statement) conn.createStatement();
				stmt.executeUpdate("DELETE FROM employes  WHERE SerialNumber='" + id + "'");
				return true;
			}

		} catch (SQLException ex) {
			System.out.println("555 " + ex.getMessage());
			return false;
		} finally {
			try {
				if (conn != null)
					conn.close();
				if (stmt != null)
					stmt.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
			}
		}
		return false;

	}

	public String newCode(String id) {
		String image = null;
		Statement stmt = null;
		Connection conn = connect();
		try {
			if (conn != null) {
				stmt = (Statement) conn.createStatement();
				ResultSet rs, rsCode, rsEmployee;
				String newCode;
				stmt.executeQuery("SELECT COUNT(*) as total FROM Employes WHERE SerialNumber='" + id + "'");
				rsEmployee = stmt.getResultSet();
				rsEmployee.first();
				if (rsEmployee.getInt("total") != 1)
					return null;
				do {
					newCode = Utils.randomCodeGen();
					stmt.executeQuery("SELECT COUNT(*) as total FROM auth WHERE Code='" + newCode + "'");
					rsCode = stmt.getResultSet();
					rsCode.first();
				} while (rsCode.getInt("total") == 1);
				stmt.executeQuery("SELECT COUNT(*) as total FROM auth WHERE IdEmployee='" + id + "'");
				rs = stmt.getResultSet();
				rs.first();

				if (rs.getInt("total") == 0) {
					stmt.executeUpdate("INSERT INTO auth (IdEmployee, Code) VALUES('" + id + "','" + newCode + "')");
				} else {
					stmt.executeUpdate("UPDATE auth SET Code='" + newCode + "' WHERE IdEmployee='" + id + "'");
				}
				image = Utils.writeQRCode(newCode, id);
			}

		} catch (SQLException ex) {
			System.out.println("666 " + ex.getMessage());
		} finally {
			try {
				if (conn != null)
					conn.close();
				if (stmt != null)
					stmt.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
			}
		}

		return image;

	}

	public ArrayList<Local> getAllLocals() {
		Statement stmt = null;
		Connection conn = connect();
		ArrayList<Local> locals = new ArrayList<Local>();
		try {
			if (conn != null) {
				stmt = (Statement) conn.createStatement();
				stmt.executeQuery("SELECT * from locals");
				ResultSet rs = stmt.getResultSet();
				while (rs.next()) {
					String id = rs.getString("Id");
					String authGrade = rs.getString("AuthGrade");
					String name = rs.getString("Name");
					locals.add(new Local(id, name, authGrade));
				}
			}
		} catch (SQLException ex) {
			System.out.println("777 " + ex.getMessage());
			return null;
		} finally {
			try {
				if (conn != null)
					conn.close();
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
		Statement stmt = null;
		Connection conn = connect();
		try {
			if (conn != null) {
				stmt = (Statement) conn.createStatement();
				stmt.executeUpdate("INSERT into employes (Name, Surname, Causal, expiration) values" + " ('"
						+ visitor.getName() + "', '" + visitor.getSurname() + "', '" + visitor.getCausal() + "', '"
						+ visitor.getExpiration() + "')", Statement.RETURN_GENERATED_KEYS);

				try (ResultSet generatedKeys = stmt.getGeneratedKeys()) {
					if (generatedKeys.next()) {
						visitorId = generatedKeys.getInt(1);

						ResultSet rs;
						String code;
						do {
							code = Utils.randomCodeGen();
							stmt.executeQuery("SELECT COUNT(*) as total FROM auth WHERE Code='" + code + "'");
							rs = stmt.getResultSet();
							rs.first();
						} while (rs.getInt("total") == 1);

						stmt.executeUpdate("INSERT into auth (IdEmployee, Code) values" + " ('" + visitorId + "', '"
								+ code + "')");

						return Utils.writeQRCode(code, String.valueOf(visitorId));
					}
				}

			}
		} catch (SQLException ex) {
			System.out.println("888" + ex.getMessage());
		} finally {
			try {
				if (conn != null)
					conn.close();
				if (stmt != null)
					stmt.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
			}
		}
		return null;
	}

	public int createNewLocal(Local local) {
		Statement stmt = null;
		Connection conn = connect();
		try {
			if (conn != null) {
				stmt = (Statement) conn.createStatement();
				stmt.executeUpdate("INSERT into locals (Id, AuthGrade, Name) values ('" + local.getIdLocal() + "', '"
						+ local.getAuthGrade() + "', '" + local.getName() + "' )");
			}
		} catch (SQLIntegrityConstraintViolationException ex) {
			System.out.println("SQLException " + ex.getMessage());
			return -1;
		} catch (SQLException ex) {
			System.out.println("999 " + ex.getMessage());
			return -2;
		} finally {
			try {

				if (conn != null)
					conn.close();
				if (stmt != null)
					stmt.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
			}
		}
		return 0;
	}

	public EmployeeResponseClass createEmployee(EmployeeRequestClass temp) {
		String employeeId = null;
		Statement stmt = null;
		String code = null;
		Connection conn = connect();
		try {

			if (conn != null) {
				stmt = (Statement) conn.createStatement();
				stmt.executeUpdate("INSERT into employes (Name, Surname, AuthGrade) values" + " ('"
						+ temp.getEmployee().getName() + "', '" + temp.getEmployee().getSurname() + "', '"
						+ temp.getEmployee().getAuthLevel() + "')", Statement.RETURN_GENERATED_KEYS);

				try (ResultSet generatedKeys = stmt.getGeneratedKeys()) {
					if (generatedKeys.next()) {
						employeeId = generatedKeys.getString(1);
						temp.getEmployee().setPhoto(Utils.StoreEmployeePhoto(temp.getPhoto(), employeeId));

						ResultSet rs;
						do {
							code = Utils.randomCodeGen();
							stmt.executeQuery("SELECT COUNT(*) as total FROM auth WHERE Code='" + code + "'");
							rs = stmt.getResultSet();
							rs.first();
						} while (rs.getInt("total") == 1);

						stmt.executeUpdate("INSERT into auth (IdEmployee, Code) values" + " ('" + employeeId + "', '"
								+ code + "')");
						stmt.executeUpdate("INSERT into photos (IdEmployee, Photo) values" + " ('" + employeeId + "', '"
								+ temp.getEmployee().getPhoto() + "')");
					}
				}
				if (employeeId != null) {
					temp.getEmployee().setSerial(employeeId);
				} else
					return null;

			}
		} catch (SQLException ex) {
			System.out.println("101010 " + ex.getMessage());
			return null;
		} finally {
			try {
				if (conn != null)
					conn.close();
				if (stmt != null)
					stmt.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
			}
		}
		temp.getEmployee().setCurrentPosition("position not found");

		return new EmployeeResponseClass(temp.getEmployee(), Utils.writeQRCode(code, employeeId));
	}

	public ArrayList<Access> makeQuery(ComplexQuery query) {
		Statement stmt = null;
		Connection conn = connect();
		ResultSet results;
		ArrayList<Access> accessResults = new ArrayList<Access>();
		try {
			if (conn != null) {
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
			}
		} catch (SQLException ex) {
			System.out.println("111111 " + ex.getMessage());
			return null;
		} finally {
			try {
				if (conn != null)
					conn.close();
				if (stmt != null)
					stmt.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
			}
		}
		return accessResults;
	}

	public String login(LoginData lg, String ExpiryDate) {
		Statement stmt = null;
		String temp = null;
		ResultSet results, exists;
		Connection conn = connect();
		if (conn == null)
			return null;
		String hash = Utils.hashString(lg.getPassword());
		try {
			stmt = (Statement) conn.createStatement();
			stmt.executeQuery("SELECT COUNT(*) as total FROM users WHERE Username='" + lg.getUsername()
					+ "' and Password='" + hash + "'");
			results = stmt.getResultSet();
			results.first();
			if (results.getInt("total") == 1) {
				do {
					temp = Utils.createToken(lg.getUsername());
					stmt.executeQuery("SELECT COUNT(*) as total FROM tokens WHERE Token='" + temp + "'");
					exists = stmt.getResultSet();
					exists.first();

				} while (exists.getInt("total") == 1);

				stmt.executeUpdate("INSERT into tokens (Token, User, Expiration) values ('" + temp + "','"
						+ lg.getUsername() + "','" + ExpiryDate + "' )");
			}
		} catch (SQLException ex) {
			System.out.println("999 " + ex.getMessage());
			return null;
		} finally {
			try {
				if (conn != null)
					conn.close();
				if (stmt != null)
					stmt.close();
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
				if (conn != null)
					conn.close();
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
			}
		}
		return true;
	}

	public boolean isValidToken(String token) {
		Statement stmt = null;
		ResultSet results;
		Connection conn = connect();
		if (conn == null)
			return false;
		String time = null;
		PreparedStatement ps = null;
		Calendar cal = Calendar.getInstance();
		cal.setTime(new Date()); // sets calendar time/date
		SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
		time = sdf.format(cal.getTime());
		try {
			stmt = (Statement) conn.createStatement();
			stmt.executeQuery(
					"SELECT COUNT(*) as total FROM tokens WHERE Token='" + token + "' AND Expiration >='" + time + "'");
			results = stmt.getResultSet();
			results.first();
			if (results.getInt("total") == 1)
				return true;
			else {
				String sql = "DELETE FROM tokens WHERE Token = ? OR Expiration < ?";
				ps = conn.prepareStatement(sql);
				ps.setString(1, token);
				ps.setString(2, time);
				ps.executeUpdate();
			}
		} catch (SQLException e) {
			return false;
		} finally {
			try {
				if (conn != null)
					conn.close();
				if (stmt != null)
					stmt.close();
				if (ps != null)
					ps.close();
			} catch (SQLException e) {
				System.out.println("Error closing " + e.getMessage());
			}
		}
		return false;
	}

	// public String loginold(LoginData lg) {
	// Statement stmt = null;
	// String temp = null;
	// ResultSet results;
	// Connection conn = connect();
	// String hash = Utils.hashString(lg.getPassword());
	// try {
	// if (conn != null) {
	// stmt = (Statement) conn.createStatement();
	// stmt.executeQuery("SELECT COUNT(*) as total FROM users WHERE Username='" +
	// lg.getUsername()
	// + "' and Password='" + hash + "'");
	// results = stmt.getResultSet();
	// results.first();
	// if (results.getInt("total") == 1) {
	// do {
	// temp = Utils.createToken(lg.getUsername());
	// } while (tokens.containsValue(temp));
	// Calendar cal = Calendar.getInstance(); // creates calendar
	// cal.setTime(new Date()); // sets calendar time/date
	// cal.add(Calendar.HOUR_OF_DAY, 1); // adds one hour
	// tokens.put(lg.getUsername(), temp);
	// }
	// }
	//
	// } catch (SQLException ex) {
	// System.out.println("999 " + ex.getMessage());
	// return null;
	// } finally {
	// try {
	//
	// if (conn != null)
	// conn.close();
	// if (stmt != null)
	// stmt.close();
	// } catch (SQLException e) {
	// System.out.println("Error closing " + e.getMessage());
	// }
	// }
	// return temp;
	// }
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
				if (conn != null)
					conn.close();
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

}