package rest;


/**
 * Class that wraps the entity "Visitor", a temporary host granted a QRcode for a short period
 *
 */
public class Visitor {
	private String  Id,Name, Surname, Causal, Position;
	private String Expiration;
	
	public Visitor(String name, String surname, String causal, String expiration) {
		this.Name=name;
		this.Surname=surname;
		this.Causal=causal;
		this.Expiration=expiration;
		
	}

	public String getId() {
		return Id;
	}

	public void setId(String id) {
		Id = id;
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

	public String getPosition() {
		return Position;
	}

	public void setPosition(String position) {
		Position = position;
	}

	public String getExpiration() {
		return Expiration;
	}

	public void setExpiration(String expiration) {
		Expiration = expiration;
	}
	

}
