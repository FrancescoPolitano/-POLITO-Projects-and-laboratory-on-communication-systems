package rest;

public class EmployeeResponseClass {
	public Employee employee;
	public String qrcodeURL;


	public EmployeeResponseClass(Employee temp, String QRCode) {
		this.employee=temp;
		this.qrcodeURL=QRCode;
	}
	public Employee getEmployee() {
		return employee;
	}
	public void setEmployee(Employee employee) {
		this.employee = employee;
	}
	public String getQrcodeURL() {
		return qrcodeURL;
	}
	public void setQrcodeURL(String qrcodeURL) {
		this.qrcodeURL = qrcodeURL;
	}
}
