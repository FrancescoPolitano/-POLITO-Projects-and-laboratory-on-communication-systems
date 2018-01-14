package rest;


/**
 * Class that wraps the entity "Visitor", a temporary host granted a QRcode for a short period
 *
 */
public class Visitor {
	private String  Serial,Name, Surname, Causal,Expiration, AuthLevel,CurrentPosition;
	
	public Visitor(String name, String surname, String causal, String expiration,String AuthLevel,String CurrentPosition) {
		this.Name=name;
		this.Surname=surname;
		this.Causal=causal;
		this.Expiration=expiration;
		this.CurrentPosition = CurrentPosition;
		this.AuthLevel = AuthLevel;
	}

	public String getSerial() {
		return Serial;
	}

	public void setSerial(String serial) {
		Serial = serial;
	}

	public String getName() {
		return Name;
	}

	public void setName(String name) {
		Name = name;
	}

	public String getSurname() {
		return Surname;
	}

	public void setSurname(String surname) {
		Surname = surname;
	}

	public String getCausal() {
		return Causal;
	}

	public void setCausal(String causal) {
		Causal = causal;
	}

	public String getExpiration() {
		return Expiration;
	}

	public void setExpiration(String expiration) {
		Expiration = expiration;
	}

	public String getAuthLevel() {
		return AuthLevel;
	}

	public void setAuthLevel(String authLevel) {
		AuthLevel = authLevel;
	}

	public String getCurrentPosition() {
		return CurrentPosition;
	}

	public void setCurrentPosition(String currentPosition) {
		CurrentPosition = currentPosition;
	}
	
	
	

}
