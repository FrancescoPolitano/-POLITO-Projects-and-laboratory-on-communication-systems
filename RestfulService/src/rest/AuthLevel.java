package rest;

public class AuthLevel {
	private String AuthLevel;
	private int SerialNumber;

	public AuthLevel(int serial, String authLevel) {
		SerialNumber = serial;
		AuthLevel = authLevel;
	}

	public int getSerialNumber() {
		return SerialNumber;
	}

	public void setSerialNumber(int serial) {
		SerialNumber = serial;
	}

	public String getAuthLevel() {
		return AuthLevel;
	}

	public void setAuthLevel(String authLevel) {
		AuthLevel = authLevel;
	}

}
