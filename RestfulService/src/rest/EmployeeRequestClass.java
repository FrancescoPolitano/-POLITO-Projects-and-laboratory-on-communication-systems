package rest;

public class EmployeeRequestClass {
	private Employee employee;
	private byte[] photo;
	
	public EmployeeRequestClass(Employee employee, byte[] photo) {
		this.employee = employee;
		this.photo = photo;
	}

	public Employee getEmployee() {
		return employee;
	}

	public void setEmployee(Employee employee) {
		this.employee = employee;
	}

	public byte[] getPhoto() {
		return photo;
	}

	public void setPhoto(byte[] photo) {
		this.photo = photo;
	}
	
}
