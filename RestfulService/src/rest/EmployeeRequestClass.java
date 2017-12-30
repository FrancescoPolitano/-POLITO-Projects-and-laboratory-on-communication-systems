package rest;

import java.awt.image.BufferedImage;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.IOException;

import javax.imageio.ImageIO;


import com.google.gson.Gson;

public class EmployeeRequestClass {
	private Employee Employee;
	private byte[] Photo;
	
	public EmployeeRequestClass(Employee employee, byte[] photo) {
		this.Employee = employee;
		this.Photo = photo;
	}

	public EmployeeRequestClass() {
		this.Employee= new Employee();
}

	public Employee getEmployee() {
		return Employee;
	}

	public void setEmployee(Employee employee) {
		this.Employee = employee;
	}

	public byte[] getPhoto() {
		return Photo;
	}

	public void setPhoto(byte[] photo) {
		this.Photo = photo;
	}
	public static EmployeeRequestClass testing() {
		EmployeeRequestClass req = new EmployeeRequestClass();
		req.Employee.setAuthLevel("1");
		req.Employee.setName("Franco");
		req.Employee.setSurname("pippo");

		  BufferedImage image = null;
		try {
			File myFile= new File("C:\\Users\\Administrator\\Desktop\\myfile.jpg");
			image = ImageIO.read(myFile);
		} catch (IOException e1) {
			// TODO Auto-generated catch block
			System.out.println(e1.getMessage());		}
	      ByteArrayOutputStream baos = new ByteArrayOutputStream();
	      try {
			ImageIO.write(image, "jpg", baos);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			System.out.println(e.getMessage());		}
		
	      byte[] encodedImage = baos.toByteArray();

		req.Photo= encodedImage;
		Gson gson = new Gson();
		String Json = gson.toJson(req);
		System.out.println(Json);
		return req;
	}
		
	}

