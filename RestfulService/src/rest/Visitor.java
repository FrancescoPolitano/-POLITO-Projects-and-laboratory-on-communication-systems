package rest;


/**
 * Class that wraps the entity "Visitor", a temporary host granted a QRcode for a short period
 *
 */
public class Visitor {
	private String  id,name, surname, causal, position;
	private String expiration;
	
	public Visitor(String name, String surname, String notes, String expiration) {
		
		this.name = name;
		this.surname = surname;
		this.causal = notes;
		this.expiration = expiration;
	}
	
	public String getPosition() {
		return position;
	}

	public void setPosition(String position) {
		this.position = position;
	}

	public String getId() {
		return id;
	}

	public void setId(String id) {
		this.id = id;
	}

	public String getCausal() {
		return causal;
	}

	public void setCausal(String causal) {
		this.causal = causal;
	}

	public String getExpiration() {
		return expiration;
	}

	public void setExpiration(String expiration) {
		this.expiration = expiration;
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
	public String getNotes() {
		return causal;
	}
	public void setNotes(String notes) {
		this.causal = notes;
	}
	public String getExpirationDate() {
		return expiration;
	}
	public void setExpirationDate(String expiration) {
		this.expiration = expiration;
	}
}
