package rest;

public class Employee {
	private String name, surname, id , authLevel, currentPosition;
	private String pathPhoto;
	private int serial;
	
	public Employee() {}
	
	public Employee(String name, String surname, String authLevel) {
		this.serial=serial;
		this.name=name;
		this.surname=surname;
		this.authLevel=authLevel;
	}
	public Employee(int serial,String name, String surname, String authLevel, String position) {
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
	public String getId() {
		return id;
	}
	public void setId(String id) {
		this.id = id;
	}
	public String getAuthLevel() {
		return authLevel;
	}
	public void setAuthLevel(String authLevel) {
		this.authLevel = authLevel;
	}

	public String getPathPhoto() {
		return pathPhoto;
	}

	public void setPathPhoto(String pathPhoto) {
		this.pathPhoto = pathPhoto;
	}

	public int getSerial() {
		return serial;
	}

	public void setSerial(int serial) {
		this.serial = serial;
	}

	public String getCurrentPosition() {
		return currentPosition;
	}

	public void setCurrentPosition(String currentPosition) {
		this.currentPosition = currentPosition;
	}

}
