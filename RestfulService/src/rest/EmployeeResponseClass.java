package rest;

public class EmployeeResponseClass {
	public Employee Employee;
	public String QrcodeURL;


	public EmployeeResponseClass(Employee temp, String QRCode) {
		this.Employee=temp;
		this.QrcodeURL=QRCode;
	}
	public Employee getEmployee() {
		return Employee;
	}
	public void setEmployee(Employee employee) {
		this.Employee = employee;
	}
	public String getQrcodeURL() {
		return QrcodeURL;
	}
	public void setQrcodeURL(String qrcodeURL) {
		this.QrcodeURL = qrcodeURL;
	}
}
