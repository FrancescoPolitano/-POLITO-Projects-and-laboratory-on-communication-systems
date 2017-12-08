package rest;

public class Local {
	private char[] IdLocal;
	private String name;
	private String authGrade;
	
	public Local(char[] id,String name, String authGrade) {
		this.IdLocal=id;
		this.name=name;
		this.authGrade=authGrade;
	}
	public char[] getIdLocal() {
		return IdLocal;
	}
	public void setIdLocal(char[] idLocal) {
		IdLocal = idLocal;
	}
	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	public String getAuthGrade() {
		return authGrade;
	}
	public void setAuthGrade(String authGrade) {
		this.authGrade = authGrade;
	}
	
}
