package rest;

import java.util.ArrayList;
import java.util.Date;
import java.util.Iterator;

public class ComplexQuery {
private ArrayList<String> employes;
private ArrayList<String> locals;
private Date initial, end;

public ComplexQuery(ArrayList<String> employes, ArrayList<String> locals, Date initial, Date end) {
	this.employes = employes;
	this.locals = locals;
	this.initial = initial;
	this.end = end;
}
//public ComplexQuery() {
//	employes = new ArrayList<String>();
//	locals= new ArrayList<String> ();
//	employes.add("3");
//	employes.add("4");
//	locals.add("BOSSDESK");
//	initial= new Date(11,12,2017);
//	end= new Date(16,12,2017);
//	System.out.println(toValidSQLQuery());
//}

public ArrayList<String> getEmployes() {
	return employes;
}

public void setEmployes(ArrayList<String> employes) {
	this.employes = employes;
}

public ArrayList<String> getLocals() {
	return locals;
}

public void setLocals(ArrayList<String> locals) {
	this.locals = locals;
}

public Date getInitial() {
	return initial;
}

public void setInitial(Date initial) {
	this.initial = initial;
}

public Date getEnd() {
	return end;
}

public void setEnd(Date end) {
	this.end = end;
}
public String toValidSQLQuery() {
	StringBuilder sb = new StringBuilder();
	sb.append("SELECT l.Name, a.TimeS, e.SerialNumber, e.Name, e.Surname, a.Result "
			+ "FROM locals l ,employes e ,accesses a  "
			+ "WHERE (");
	
	
	
	   for (Iterator<String> it = employes.iterator(); it.hasNext();) {
           String element = it.next();
           sb.append("e.SerialNumber='"+element+"'");
           if(it.hasNext()){
               sb.append( " OR ");
           }
       }
	sb.append(") AND (");

	   for (Iterator<String> it = locals.iterator(); it.hasNext();) {
           String element = it.next();
           sb.append("l.Id='"+element+"'");
           if(it.hasNext()){
               sb.append(" OR ");
           }
       }
	
	sb.append(") AND (");
	
	if(getEnd()!=null)
		sb.append("a.TimeS>'"+getInitial()+"' AND a.TimeS<'"+getEnd()+"')");
	else
		sb.append("a.TimeS='"+getInitial()+"')");


	return sb.toString();
	
	}
}
