package rest;

import java.awt.image.BufferedImage;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.SQLIntegrityConstraintViolationException;
import java.sql.Statement;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.Random;

public class Database {
	static Connection conn;

	public static void init() throws SQLException {
		try {
			connect();
		} catch (InstantiationException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IllegalAccessException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (ClassNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

	private static void connect()
			throws InstantiationException, IllegalAccessException, ClassNotFoundException, SQLException {
		String user = "root";
		String password = user;
		try {
			Class.forName("com.mysql.jdbc.Driver").newInstance();

		} catch (ClassNotFoundException ex) {
			System.out.println("Error: unable to load driver class!");
			System.exit(1);
		} catch (IllegalAccessException ex) {
			System.out.println("Error: access problem while loading!");
			System.exit(2);
		} catch (InstantiationException ex) {
			System.out.println("Error: unable to instantiate driver!");
			System.exit(3);
		}
		conn = DriverManager.getConnection("jdbc:mysql://127.0.0.1:3306/paldb", user, password);

		if (conn != null) {
			System.out.println("Connected to the database");
		}
	}

	public static ArrayList<Employee> getAllEmployes() {
		ArrayList<Employee> list = new ArrayList<Employee>();
		try {
			if (conn != null) {
				Statement stmt = (Statement) conn.createStatement();
				stmt.executeQuery("SELECT e.*, l.Name, max(a.TimeS)\r\n" + "from employes e , locals l, accesses a \r\n"
						+ "where e.Causal IS NULL and a.IdEmployee=e.SerialNumber and l.Id=a.IdLocal\r\n" + "GROUP BY e.SerialNumber");
				ResultSet rs = stmt.getResultSet();
				while (rs.next()) {
					String serial = rs.getString("SerialNumber");
					String name = rs.getString("Name");
					String surname = rs.getString("Surname");
					String auth = rs.getString("AuthGrade");
					String position = rs.getString("l.Name");

					Employee temp = new Employee(serial, name, surname, auth, "ops");
					list.add(temp);
				}
				stmt.executeQuery("SELECT e.* from employes e\r\n"
						+ "where e.Causal IS NULL and e.SerialNumber not  in(SELECT e.SerialNumber from employes e , locals l, accesses a \r\n"
						+ "						where a.IdEmployee=e.SerialNumber and l.Id=a.IdLocal GROUP BY e.SerialNumber)\r\n"
						+ "");
				rs = stmt.getResultSet();
				while (rs.next()) {
					String serial = rs.getString("SerialNumber");
					String name = rs.getString("Name");
					String surname = rs.getString("Surname");
					String auth = rs.getString("AuthGrade");
					String position = "No position found";

					Employee temp = new Employee(serial, name, surname, auth, position);
					list.add(temp);

				}
				return list;
			}
		} catch (SQLException ex) {
			System.out.println("Error: access problem while loading!");
			System.exit(2);
		}
		return list;
	}

	public static ArrayList<Visitor> getAllVisitors() {
		ArrayList<Visitor> list = new ArrayList<Visitor>();
		try {
			if (conn != null) {
				Statement stmt = (Statement) conn.createStatement();
				stmt.executeQuery("SELECT e.*, l.Name, max(a.TimeS)\r\n" + "from employes e , locals l, accesses a \r\n"
						+ "where e.Causal IS NOT NULL and a.IdEmployee=e.SerialNumber and l.Id=a.IdLocal\r\n" + "GROUP BY e.SerialNumber");
				ResultSet rs = stmt.getResultSet();
				while (rs.next()) {
					String serial = rs.getString("SerialNumber");
					String name = rs.getString("Name");
					String surname = rs.getString("Surname");
					String causal = rs.getString("Causal");
					String expiration = rs.getString("Expiration");
					String position = rs.getString("l.Name");

					Visitor temp = new Visitor(name, surname, causal, expiration) ;
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
					Visitor temp = new Visitor(name, surname, causal, expiration) ;
					temp.setPosition(position);
					temp.setId(serial);
					list.add(temp);

				}
				return list;
			}
		} catch (SQLException ex) {
			System.out.println("Error: access problem while loading!");
			System.exit(2);
		}
		return list;
	}

	public static Employee getEmployee(String id) throws SQLException {
		Employee temp = null;
		try {
			if (conn != null) {
				Statement stmt = (Statement) conn.createStatement();
				stmt.executeQuery("SELECT e.*, l.Name, a.TimeS\r\n" + "from employes e , locals l, accesses a \r\n"
						+ "where a.TimeS= (select max(TimeS) from accesses a where a.IdEmployee='" + id + "') \r\n"
						+ "and a.IdEmployee=e.SerialNumber and a.Result='true' and l.Id=a.IdLocal\r\n" + "");
				ResultSet rs = stmt.getResultSet();
				while (rs.next()) {
					String serial = rs.getString("SerialNumber");
					String name = rs.getString("Name");
					String surname = rs.getString("Surname");
					String auth = rs.getString("AuthGrade");
					String position = rs.getString("l.Name");
					temp = new Employee(serial, name, surname, auth, position);

				}
				if(temp!=null)
				return temp;
				else {
				stmt.executeQuery("SELECT e.* from employes e where e.SerialNumber='"+id+"' and e.SerialNumber  not  in(SELECT a.IdEmployee from accesses a) ");
				rs = stmt.getResultSet();
				while (rs.next()) {
					String serial = rs.getString("SerialNumber");
					String name = rs.getString("Name");
					String surname = rs.getString("Surname");
					String auth = rs.getString("AuthGrade");
					String position = "position not found";
					temp = new Employee(serial, name, surname, auth, position);
					temp.setCurrentPosition(position);
					temp.setSerial(serial);

				}
				return temp;
			}
			}
		} catch (SQLException ex) {
			System.out.println(ex.getMessage());
			System.exit(2);
		}
		return temp;
	}

	public static boolean isAuth(String local, String code) {
		Integer serial = null;
		int authGrade = 0, requestedGrade = 0;
		try {
			if (conn != null) {
				Statement stmt = (Statement) conn.createStatement();
				stmt.executeQuery("select  SerialNumber, e.AuthGrade , l.AuthGrade "
						+ "from employes e , auth a , locals l" + " where a.Code ='" + code
						+ "' and a.IdEmployee= e.SerialNumber and l.Id='" + local + "'");
				ResultSet rs = stmt.getResultSet();
				while (rs.next()) {
					serial = rs.getInt("SerialNumber");
					authGrade = rs.getInt("e.Authgrade");
					requestedGrade = rs.getInt("l.Authgrade");
				}
				if (serial == null) {
					return false;
				}
				if (authGrade < requestedGrade) {
					stmt.executeUpdate("INSERT INTO accesses (IdEmployee,IdLocal,Result) VALUES ('" + serial + "','"
							+ local + "', 'false')");

					return false;
				}

				else {

					stmt.executeUpdate("INSERT INTO accesses (IdEmployee,IdLocal,Result) VALUES ('" + serial + "','"
							+ local + "', 'true')");

					return true;
				}

			}
		} catch (SQLException ex) {
			System.out.println(ex.getMessage());
			System.exit(2);
		}
		return false;
	}

	public static boolean deleteEmployee(String id) {
		try {
			if (conn != null) {
				Statement stmt = (Statement) conn.createStatement();
				stmt.executeUpdate("DELETE FROM employes  WHERE SerialNumber='" + id + "'");
				return true;
			}

		} catch (SQLException ex) {
			System.out.println(ex.getMessage());
			return false;
		}
		return false;

	}

	public static String newCode(String id) {
		String image = null;
		try {
			if (conn != null) {
				Statement stmt = (Statement) conn.createStatement();
				ResultSet rs;
				String newCode;
				do {
					newCode = Utils.randomCodeGen();
					stmt.executeQuery("SELECT COUNT(*) as total FROM auth WHERE Code='" + newCode + "'");
					rs = stmt.getResultSet();
					rs.first();
				} while (rs.getInt("total") == 1);
				stmt.executeQuery("SELECT COUNT(*) as total FROM auth WHERE IdEmployee='" + id + "'");
				rs = stmt.getResultSet();
				rs.first();

				if(rs.getInt("total")==0) {
					stmt.executeUpdate("INSERT INTO auth (IdEmployee, Code) VALUES('"+id + "','" + newCode + "')");
				}
				else {
				stmt.executeUpdate("UPDATE auth SET Code='" + newCode + "' WHERE IdEmployee='" + id + "'");
				}
				image = Utils.writeQRCode(newCode, id);
				return image;
			}

		} catch (SQLException ex) {
			System.out.println(ex.getMessage());
			return null;
		}
		return image;

	}

	public static ArrayList<Local> getAllLocals() {
		ArrayList<Local> locals = new ArrayList<Local>();
		try {
			if (conn != null) {
				Statement stmt = (Statement) conn.createStatement();
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
			System.out.println("Error: access problem while loading!");
			System.out.println("ECCEZIONE " + ex.getMessage());
			return null;
		}
		return locals;
	}

	public static String createVisitor(Visitor visitor) {
		Integer visitorId = null;
		try {

			if (conn != null) {
				Statement stmt = (Statement) conn.createStatement();
				stmt.executeUpdate("INSERT into employes (Name, Surname, Causal, expiration) values" + " ('"
						+ visitor.getName() + "', '" + visitor.getSurname() + "', '" + visitor.getNotes() + "', '"
						+ visitor.getExpirationDate() + "')", Statement.RETURN_GENERATED_KEYS);

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
					}
				}

			}
		} catch (SQLException ex) {
			System.out.println(ex.getMessage());
			return "error";
		}
		return null;

	}

	public static int createNewLocal(Local local) {
		try {
			if (conn != null) {
				Statement stmt = (Statement) conn.createStatement();
				stmt.executeUpdate("INSERT into locals (Id, AuthGrade, Name) values ('" + local.getIdLocal() + "', '"
						+ local.getAuthGrade() + "', '" + local.getName() + "' )");
			}
		} catch (SQLIntegrityConstraintViolationException ex) {
			System.out.println("SQLException " + ex.getMessage());
			return -1;
		} catch (SQLException ex) {
			System.out.println("SQLException " + ex.getMessage());
			return -2;
		}
		return 0;
	}

	public static EmployeeResponseClass createEmployee(EmployeeRequestClass temp) {
		String employeeId = null;
		String code = null;
		try {

			if (conn != null) {
				Statement stmt = (Statement) conn.createStatement();
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
			System.out.println(ex.getMessage());
			return null;
		}

		return new EmployeeResponseClass(temp.getEmployee(), Utils.writeQRCode(code, employeeId));
	}

	public static ArrayList<Access> makeQuery(ComplexQuery query) {
		ResultSet results;
		ArrayList<Access> accessResults = new ArrayList<Access>();
		try {
			if (conn != null) {
				Statement stmt = (Statement) conn.createStatement();
				stmt.executeQuery(query.toValidSQLQuery());
				results = stmt.getResultSet();

				Access temp;

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
			System.out.println(ex.getMessage());
			return null;
		}
		return accessResults;
	}
}
