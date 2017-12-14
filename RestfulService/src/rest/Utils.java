package rest;
import java.awt.image.BufferedImage;
import java.io.ByteArrayInputStream;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.nio.file.Files;
import java.util.Base64;
import java.util.Hashtable;
import java.util.Random;

import javax.imageio.ImageIO;

import com.google.zxing.BarcodeFormat;
import com.google.zxing.EncodeHintType;
import com.google.zxing.WriterException;
import com.google.zxing.common.BitMatrix;
import com.google.zxing.common.ByteMatrix;
import com.google.zxing.qrcode.QRCodeWriter;
import com.google.zxing.qrcode.decoder.ErrorCorrectionLevel;



/**
 *Contains some useful functions
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
			// exit the method
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
		String URI= "C:\\Users\\franc\\Desktop\\ServerData\\"+UserId+".jpg";
		String URL= "/images/"+data+".jpg";
		try {
			fos = new FileOutputStream(URI);
		} catch (FileNotFoundException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
		}
		try {
			ImageIO.write(image, "png", fos);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		return URL;
		
	}

	public static String StoreEmployeePhoto(byte[] fileContent, String id ) {
		BufferedImage img;
		String URL= "/images/profiles/"+id+".jpg";
		//Converto il byte array in una buffered image
		 ByteArrayInputStream bais = new ByteArrayInputStream(fileContent);
		    try {
		         img= ImageIO.read(bais);
		    } catch (IOException e) {
		        throw new RuntimeException(e);
		    }

		FileOutputStream fos = null;
		String URI= "C:\\Users\\franc\\Desktop\\ServerData\\profiles\\"+id+".jpg";
		try {
			fos = new FileOutputStream(URI);
		} catch (FileNotFoundException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
		}
		try {
			ImageIO.write(img, "png", fos);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		return URL;
				
	}
}
