package rest;

/**
 * Class that represents an employee, his unique features and his last known position
 *
 */
public class Employee {
	private String name, surname , authLevel, currentPosition;
	private String Photo;
	private String serial;
	
	public Employee() {}
	
	public Employee(String name, String surname, String authLevel) {
		this.name=name;
		this.surname=surname;
		this.authLevel=authLevel;
	}
	public Employee(String serial,String name, String surname, String authLevel, String position) {
		this.serial=serial;
		this.name=name;
		this.surname=surname;
		this.authLevel=authLevel;
		this.currentPosition=position;
	}

	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	public String getSurname() {
		return surname;
	}
	public void setSurname(String surname) {
		this.surname = surname;
	}
	
	public String getAuthLevel() {
		return authLevel;
	}
	public void setAuthLevel(String authLevel) {
		this.authLevel = authLevel;
	}

	public String getPhoto() {
		return Photo;
	}

	public void setPhoto(String Photo) {
		this.Photo = Photo;
	}

	public String getSerial() {
		return serial;
	}

	public void setSerial(String employeeId) {
		this.serial = employeeId;
	}

	public String getCurrentPosition() {
		return currentPosition;
	}

	public void setCurrentPosition(String currentPosition) {
		this.currentPosition = currentPosition;
	}

}
