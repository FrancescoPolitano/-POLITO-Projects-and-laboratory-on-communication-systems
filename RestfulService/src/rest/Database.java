package rest;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
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
				// stmt.executeQuery("select a.* from employes a ");
				stmt.executeQuery("SELECT e.*, l.Name, max(a.TimeS)\r\n" + "from employes e , locals l, accesses a \r\n"
						+ "where a.IdEmployee=e.SerialNumber and l.Id=a.IdLocal\r\n" + "GROUP BY e.SerialNumber");
				ResultSet rs = stmt.getResultSet();
				while (rs.next()) {
					int serial = rs.getInt("SerialNumber");
					String name = rs.getString("Name");
					String surname = rs.getString("Surname");
					String auth = rs.getString("AuthGrade");
					String position = rs.getString("l.Name");

					Employee temp = new Employee(serial, name, surname, auth, position);
					list.add(temp);
				}
				stmt.executeQuery("SELECT e.* from employes e\r\n"
						+ "where e.SerialNumber not  in(SELECT e.SerialNumber from employes e , locals l, accesses a \r\n"
						+ "						where a.IdEmployee=e.SerialNumber and l.Id=a.IdLocal GROUP BY e.SerialNumber)\r\n"
						+ "");
				rs = stmt.getResultSet();
				while (rs.next()) {
					int serial = rs.getInt("SerialNumber");
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

	public static Employee getEmployee(String id) throws SQLException {
		Employee temp = null;
		try {
			if (conn != null) {
				Statement stmt = (Statement) conn.createStatement();
				stmt.executeQuery("SELECT e.*, l.Name, a.TimeS\r\n" + "from employes e , locals l, accesses a \r\n"
						+ "where a.TimeS= (select max(TimeS) from accesses a where a.IdEmployee='" + id + "') \r\n"
						+ "and a.IdEmployee=e.SerialNumber and l.Result='true' and l.Id=a.IdLocal\r\n" + "");
				ResultSet rs = stmt.getResultSet();
				while (rs.next()) {
					int serial = rs.getInt("SerialNumber");
					String name = rs.getString("Name");
					String surname = rs.getString("Surname");
					String auth = rs.getString("AuthGrade");
					String position = rs.getString("l.Name");
					temp = new Employee(serial, name, surname, auth, position);

				}
				return temp;
			}
		} catch (SQLException ex) {
			System.out.println("Error: access problem while loading!");
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

		try {
			if (conn != null) {
				Statement stmt = (Statement) conn.createStatement();
				String newCode = randomCodeGen();
				stmt.executeUpdate(
						"UPDATE auth SET Code='" + newCode + "' WHERE IdEmployee='" + id + "'");
				return newCode;
			}

		} catch (SQLException ex) {
			System.out.println(ex.getMessage());
			return null;
		}
		return null;

	}

	private static String randomCodeGen() {
		char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".toCharArray();
		Random random = new Random();
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < 12; i++) {
			char c = chars[random.nextInt(chars.length)];
			sb.append(c);
		}
		return sb.toString();
	}

}