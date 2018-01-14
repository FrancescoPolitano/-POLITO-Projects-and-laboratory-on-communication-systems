package rest;

public class VisitorResponseClass {
	public Visitor visitor;
	public String QrcodeURL;

	public VisitorResponseClass(Visitor v, String QRCode) {
		visitor = v;
		QrcodeURL = QRCode;
	}

	public Visitor getVisitor() {
		return visitor;
	}

	public void setVisitor(Visitor v) {
		visitor = v;
	}

	public String getQrcodeURL() {
		return QrcodeURL;
	}

	public void setQrcodeURL(String qrcodeURL) {
		this.QrcodeURL = qrcodeURL;
	}

}
