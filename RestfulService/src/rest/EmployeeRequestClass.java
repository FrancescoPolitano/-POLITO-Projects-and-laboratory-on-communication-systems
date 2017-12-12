package rest;

public class EmployeeRequestClass {
	public Employee employee;
	public byte[] photo;
	
	public EmployeeRequestClass(Employee employee, byte[] photo) {
		super();
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
