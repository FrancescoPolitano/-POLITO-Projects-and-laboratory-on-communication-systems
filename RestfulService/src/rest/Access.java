package rest;
import java.sql.Timestamp;



/**
 * Class that represents an access in a local or a attempted one 
 *
 */
public class Access {
	private String employeeId,employeeName, employeeSurname, localName;
	private Timestamp time;
	private Boolean result;
	
	public Access() {}

	public Access(String employeeId, String employeeName, String employeeSurname, String localName, Timestamp time,
			Boolean result) {
		this.employeeId = employeeId;
		this.employeeName = employeeName;
		this.employeeSurname = employeeSurname;
		this.localName = localName;
		this.time = time;
		this.result = result;
	}
	public String getEmployeeId() {
		return employeeId;
	}
	public void setEmployeeId(String employeeId) {
		this.employeeId = employeeId;
	}
	public String getEmployeeName() {
		return employeeName;
	}
	public void setEmployeeName(String employeeName) {
		this.employeeName = employeeName;
	}
	public String getEmployeeSurname() {
		return employeeSurname;
	}
	public void setEmployeeSurname(String employeeSurname) {
		this.employeeSurname = employeeSurname;
	}
	public String getLocalName() {
		return localName;
	}
	public void setLocalName(String localName) {
		this.localName = localName;
	}
	public Timestamp getTime() {
		return time;
	}
	public void setTime(Timestamp time) {
		this.time = time;
	}
	public Boolean getResult() {
		return result;
	}
	public void setResult(Boolean result) {
		this.result = result;
	}

}
