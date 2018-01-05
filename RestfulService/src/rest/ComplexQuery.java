package rest;

import java.util.ArrayList;
import java.util.Date;
import java.util.Iterator;

public class ComplexQuery {
	private ArrayList<String> Employees;
	private ArrayList<String> Rooms;
	private Date Initial, End;

	public ComplexQuery(ArrayList<String> employees, ArrayList<String> rooms, Date initial, Date end) {
		Employees = new ArrayList<String>(employees);
		Rooms = new ArrayList<String>(rooms);
		Initial = initial;
		End = end;
	}

	public ArrayList<String> getEmployees() {
		return Employees;
	}

	public void setEmployees(ArrayList<String> employees) {
		Employees = employees;
	}

	public ArrayList<String> getRooms() {
		return Rooms;
	}

	public void setRooms(ArrayList<String> rooms) {
		Rooms = rooms;
	}

	public Date getInitial() {
		return Initial;
	}

	public void setInitial(Date initial) {
		Initial = initial;
	}

	public Date getEnd() {
		return End;
	}

	public void setEnd(Date end) {
		End = end;
	}

	public String toValidSQLQuery() {
		StringBuilder sb = new StringBuilder();
		sb.append("SELECT l.Name, a.TimeS, e.SerialNumber, e.Name, e.Surname, a.Result "
				+ "FROM locals l ,employes e ,accesses a  " + "WHERE ");

		StringBuilder employees = new StringBuilder("");
		for (Iterator<String> it = Employees.iterator(); it.hasNext();) {
			String element = it.next();
			employees.append("e.SerialNumber='" + element + "'");
			if (it.hasNext()) {
				sb.append(" OR ");
			}
		}
		if (!employees.toString().equals("")) {
			sb.append(" ( ").append(employees.toString()).append(" ) AND ");
		}

		StringBuilder rooms = new StringBuilder("");
		for (Iterator<String> it = Rooms.iterator(); it.hasNext();) {
			String element = it.next();
			rooms.append("l.Id='" + element + "'");
			if (it.hasNext()) {
				sb.append(" OR ");
			}
		}

		if (!rooms.toString().equals(""))
			sb.append(" ( ").append(rooms.toString()).append(" ) AND ");

		if (getEnd() != null)
			sb.append("( a.TimeS >'" + getInitial() + "' AND a.TimeS <'" + getEnd() + "')");
		else
			sb.append("( a.TimeS='" + getInitial() + "')");

		return sb.toString();
	}
}
