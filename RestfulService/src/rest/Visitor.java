package rest;

import java.sql.Date;

public class Visitor {
	private String name, surname, causal;
	private String expiration;
	
	public Visitor(String name, String surname, String notes, String expiration) {
		
		this.name = name;
		this.surname = surname;
		this.causal = notes;
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
