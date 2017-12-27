package rest;

import java.util.Date;

public class TokenValidity {
	String token;
	Date validity;
	public TokenValidity(String token, Date validity) {
		super();
		this.token = token;
		this.validity = validity;
	}
	public String getToken() {
		return token;
	}
	public void setToken(String token) {
		this.token = token;
	}
	public Date getValidity() {
		return validity;
	}
	public void setValidity(Date validity) {
		this.validity = validity;
	}
}
