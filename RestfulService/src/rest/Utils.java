package rest;

import java.awt.image.BufferedImage;
import java.io.ByteArrayInputStream;
import java.io.FileOutputStream;
import java.io.UnsupportedEncodingException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Properties;
import java.util.Random;

import javax.activation.DataHandler;
import javax.activation.DataSource;
import javax.activation.FileDataSource;
import javax.imageio.ImageIO;
import javax.mail.BodyPart;
import javax.mail.Message;
import javax.mail.PasswordAuthentication;
import javax.mail.Session;
import javax.mail.Transport;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeBodyPart;
import javax.mail.internet.MimeMessage;
import javax.mail.internet.MimeMultipart;

import com.google.zxing.common.ByteMatrix;
import com.google.zxing.qrcode.QRCodeWriter;

/**
 * Contains some useful functions
 *
 */

public class Utils {

	/**
	 * Creates a random alphanumeric string
	 *
	 */
	static String randomCodeGen() {
		char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".toCharArray();
		Random random = new Random();
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < 12; i++) {
			char c = chars[random.nextInt(chars.length)];
			sb.append(c);
		}
		return sb.toString();
	}

	/**
	 * Creates the qrCode and store it into the filesystem
	 *
	 */
	public static String writeQRCode(String data, String UserId) {

		// get a byte matrix for the data
		ByteMatrix matrix;
		com.google.zxing.Writer writer = new QRCodeWriter();

		try {
			matrix = writer.encode(data, com.google.zxing.BarcodeFormat.QR_CODE, 256, 256);
		} catch (com.google.zxing.WriterException e) {
			System.out.println(e.getMessage());
			return null;
		}

		// generate an image from the byte matrix
		int width = matrix.getWidth();
		int height = matrix.getHeight();

		byte[][] array = matrix.getArray();

		// create buffered image to draw to
		BufferedImage image = new BufferedImage(width, height, BufferedImage.TYPE_INT_RGB);

		// iterate through the matrix and draw the pixels to the image
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				int grayValue = array[y][x] & 0xff;
				image.setRGB(x, y, (grayValue == 0 ? 0 : 0xFFFFFF));
			}
		}

		// write the image to the output stream
		FileOutputStream fos = null;
		String URI = "C:\\Users\\Administrator\\Desktop\\ServerData\\" + UserId + ".jpg";
		String URL = "/images/" + UserId + ".jpg";
		try {
			fos = new FileOutputStream(URI);
			ImageIO.write(image, "png", fos);
			fos.close();
		} catch (Exception e) {
			return null;
		}
		return URL;
	}

	public static String StoreEmployeePhoto(byte[] fileContent, String id) {
		BufferedImage img;
		String URL = "/images/profiles/" + id + ".jpg";
		// Converto il byte array in una buffered image
		ByteArrayInputStream bais = new ByteArrayInputStream(fileContent);
		FileOutputStream fos = null;
		String URI = "C:\\Users\\Administrator\\Desktop\\ServerData\\profiles\\" + id + ".jpg";
		try {
			img = ImageIO.read(bais);
			fos = new FileOutputStream(URI);
			ImageIO.write(img, "png", fos);
		} catch (Exception e) {
			return null;
		}
		return URL;
	}

	public static String hashString(String message) {

		MessageDigest digest = null;
		try {
			digest = MessageDigest.getInstance("SHA-256");
		} catch (NoSuchAlgorithmException e) {
			e.printStackTrace();
		}
		byte[] hashedBytes = null;
		try {
			hashedBytes = digest.digest(message.getBytes("UTF-8"));
		} catch (UnsupportedEncodingException e) {
			e.printStackTrace();
		}
		return convertByteArrayToHexString(hashedBytes);
	}

	private static String convertByteArrayToHexString(byte[] arrayBytes) {
		StringBuffer stringBuffer = new StringBuffer();
		for (int i = 0; i < arrayBytes.length; i++)
			stringBuffer.append(Integer.toString((arrayBytes[i] & 0xff) + 0x100, 16).substring(1));

		return stringBuffer.toString();
	}

	public static String createToken(String string) {
		String token = randomCodeGen();
		return token;
	}

	public static int sendEmail(String sendTo, String employeeId) {

		Properties props = new Properties();
		props.put("mail.smtp.auth", "true");
		props.put("mail.smtp.starttls.enable", "true");
		props.put("mail.smtp.host", "smtp.gmail.com");
		props.put("mail.smtp.port", "587");

		Session session = Session.getInstance(props, new javax.mail.Authenticator() {
			protected PasswordAuthentication getPasswordAuthentication() {
				return new PasswordAuthentication(Constants.email_address, Constants.email_password);
			}
		});

		try {
			Message message = new MimeMessage(session);
			message.setFrom(new InternetAddress(Constants.email_address));
			message.setRecipients(Message.RecipientType.TO, InternetAddress.parse(sendTo));
			message.setSubject("New Access Code");
			MimeMultipart multipart = new MimeMultipart("related");

			BodyPart messageBodyPart = new MimeBodyPart();
			String htmlText = "<img src=\"cid:image\">";
			messageBodyPart.setContent(htmlText, "text/html");

			multipart.addBodyPart(messageBodyPart);

			messageBodyPart = new MimeBodyPart();
			DataSource fds = new FileDataSource("C:\\Users\\Administrator\\Desktop\\ServerData\\" + employeeId + ".jpg");
			messageBodyPart.setDataHandler(new DataHandler(fds));
			messageBodyPart.setHeader("Content-ID", "<image>");

			multipart.addBodyPart(messageBodyPart);
			message.setContent(multipart);
			Transport.send(message);
		} catch (Exception e) {
			e.printStackTrace();
			return -1;
		}
		return 0;
	}
	public static int sendConfirmationEmail(String sendTo, String employeeId) {

		Properties props = new Properties();
		props.put("mail.smtp.auth", "true");
		props.put("mail.smtp.starttls.enable", "true");
		props.put("mail.smtp.host", "smtp.gmail.com");
		props.put("mail.smtp.port", "587");

		Session session = Session.getInstance(props, new javax.mail.Authenticator() {
			protected PasswordAuthentication getPasswordAuthentication() {
				return new PasswordAuthentication(Constants.email_address, Constants.email_password);
			}
		});

		try {
			Message message = new MimeMessage(session);
			message.setFrom(new InternetAddress(Constants.email_address));
			message.setRecipients(Message.RecipientType.TO, InternetAddress.parse(sendTo));
			message.setSubject("EasyAccess: Please confirm your email address");
			MimeMultipart multipart = new MimeMultipart("related");

			BodyPart messageBodyPart = new MimeBodyPart();
			String htmlText = "If you don't know the origin of this email please ignore it.<br>"
					+ "<a href=\"http://13.59.45.212:8080/RestfulService/resources/confirm/"+employeeId+"\">Click here to confirm</a>";
			messageBodyPart.setContent(htmlText, "text/html");
			multipart.addBodyPart(messageBodyPart);
			message.setContent(multipart);
			Transport.send(message);
		} catch (Exception e) {
			e.printStackTrace();
			return -1;
		}
		return 0;
	}

}
