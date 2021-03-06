package rest;

/**
 * Class that represents an employee, his unique features and his last known
 * position
 *
 */
public class Employee {
	private String Name, Surname, AuthLevel, CurrentPosition,PathPhoto, Serial, Email;

	public Employee() {
	}

	public Employee(String name, String surname, String authLevel, String email) {
		this.Name = name;
		this.Surname = surname;
		this.AuthLevel = authLevel;
		Email = email;
	}

	public Employee(String serial, String name, String surname, String authLevel, String position, String email) {
		this.Serial = serial;
		this.Name = name;
		this.Surname = surname;
		this.AuthLevel = authLevel;
		this.CurrentPosition = position;
		Email = email;
	}

	public String getEmail() {
		return Email;
	}

	public void setEmail(String email) {
		Email = email;
	}

	public String getName() {
		return Name;
	}

	public void setName(String name) {
		this.Name = name;
	}

	public String getSurname() {
		return Surname;
	}

	public void setSurname(String surname) {
		this.Surname = surname;
	}

	public String getAuthLevel() {
		return AuthLevel;
	}

	public void setAuthLevel(String authLevel) {
		this.AuthLevel = authLevel;
	}

	public String getPhoto() {
		return PathPhoto;
	}

	public void setPhoto(String Photo) {
		this.PathPhoto = Photo;
	}

	public String getSerial() {
		return Serial;
	}

	public void setSerial(String employeeId) {
		this.Serial = employeeId;
	}

	public String getCurrentPosition() {
		return CurrentPosition;
	}

	public void setCurrentPosition(String currentPosition) {
		this.CurrentPosition = currentPosition;
	}

}
