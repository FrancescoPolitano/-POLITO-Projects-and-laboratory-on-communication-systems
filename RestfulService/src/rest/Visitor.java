package rest;

import java.sql.Date;

public class Visitor {
	private String name, surname, notes;
	private Date expirationDate;
	
	public Visitor(String name, String surname, String notes, Date expirationDate) {
		super();
		this.name = name;
		this.surname = surname;
		this.notes = notes;
		this.expirationDate = expirationDate;
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
		return notes;
	}
	public void setNotes(String notes) {
		this.notes = notes;
	}
	public Date getExpirationDate() {
		return expirationDate;
	}
	public void setExpirationDate(Date expirationDate) {
		this.expirationDate = expirationDate;
	}
}
