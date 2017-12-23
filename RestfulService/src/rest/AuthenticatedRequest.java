package rest;

public class AuthenticatedRequest {
 String Token;
 Object Body;
 
public String getToken() {
	return Token;
}
public void setToken(String token) {
	Token = token;
}
public Object getBody() {
	return Body;
}
public void setBody(Object body) {
	Body = body;
}
 
}
