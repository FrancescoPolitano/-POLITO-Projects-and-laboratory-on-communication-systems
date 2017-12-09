package rest;

public class Local {

	private String name, idLocal;
	private String authGrade;
	
	public Local(String id,String name, String authGrade) {
		this.idLocal=id;
		this.name=name;
		this.authGrade=authGrade;
	}
	public String getIdLocal() {
		return idLocal;
	}
	public void setIdLocal(String idLocal) {
		idLocal = idLocal;
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
