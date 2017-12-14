package rest;


import java.sql.SQLException;
import javax.ws.rs.Consumes;
import javax.ws.rs.DELETE;
import javax.ws.rs.GET;
import javax.ws.rs.POST;
import javax.ws.rs.Path;
import javax.ws.rs.PathParam;
import javax.ws.rs.Produces;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;

import com.google.gson.Gson;

//Sets the base url
@Path("/resources")
public class Resources{
	public Resources(){
		Database.connect();
	}

	// returns all the employes
	@GET
	@Path("users/employees")
	@Produces(MediaType.TEXT_PLAIN)
	public Response sendAllEmployes() {
		Gson gson = new Gson();
//		EmployeeRequestClass.testing();
		String Json = gson.toJson(Database.getAllEmployes());
		if (Json == null)
			return Response.status(801).entity("Something goes wrong").build();
		return Response.ok(Json, MediaType.APPLICATION_JSON).build();

	}
	@GET
	@Path("users/visitors")
	@Produces(MediaType.TEXT_PLAIN)
	public Response sendAllVisitors() {
		Gson gson = new Gson();
		String Json = gson.toJson(Database.getAllVisitors());
		if (Json == null)
			return Response.status(801).entity("We're in big trubles").build();
		return Response.ok(Json, MediaType.APPLICATION_JSON).build();

	}
	// returns the employee related to the chosen id    
	@GET
	@Path("users/{id}")
	@Produces(MediaType.TEXT_PLAIN)
	public Response sendEmployee(@PathParam("id") String id) throws SQLException {
		Gson gson = new Gson();
		String Json = gson.toJson(Database.getEmployee(id));
		if (Json == null)
			return Response.status(404).entity("The resource doesn't exist").build();
		return Response.ok(Json, MediaType.APPLICATION_JSON).build();
	}

	// create a new employee and return the user and the QRcode   (not working)
	@POST
	@Path("users/employees")
	@Produces(MediaType.TEXT_PLAIN)
	public Response newEmployee(String employee) throws SQLException {
		EmployeeRequestClass temp = new Gson().fromJson(employee, EmployeeRequestClass.class);
		if (temp.getEmployee() == null)
			return Response.status(802).entity("Fucking bugged").build();

		EmployeeResponseClass newEmployee = Database.createEmployee(temp);
		if (newEmployee == null)
			return Response.status(801).entity("We're in big trubles").build();
		Gson gson = new Gson();
		String Json = gson.toJson(newEmployee);
		return Response.ok(Json).build();
	}

	// return if the code is valid or not for the selected locals
	@GET
	@Path("auth/{local}/{code}")
	@Produces(MediaType.TEXT_PLAIN)
	public Response getAccess(@PathParam("local") String door, @PathParam("code") String code) {

		return Response.ok(Database.isAuth(door, code)).build();
	}

	// create a temporary user and grant him the lowest level of permission, returns
	// the user as a json
	@POST
	@Path("users/visitors")
	public Response newVisitor(String visitor) {
		rest.Visitor temp = new Gson().fromJson(visitor, rest.Visitor.class);
		// TODO return the qrcode image
		String code=Database.createVisitor(temp);
		if(code==null) {
			return Response.status(801).entity("We're in big trubles").build();

		}
		return Response.ok(code).build();
	}

	// revoke a user QRCode and obtain a new one
	@POST
	@Path("users/new_code")
	 @Produces(MediaType.TEXT_PLAIN)
    public Response getNewCode(String id) {
		String code= Database.newCode(id);
 		if(code==null) {
			return Response.status(801).entity("We're in big trubles").build();
		}
		return Response.ok(code).build();
 		
    }

	// delete an user
	@DELETE
	@Path("users/{id}")
	public Response deleteEmployee(@PathParam("id") String id) {
		boolean result = Database.deleteEmployee(id);
		if(!result)
		return Response.status(404).entity("Nothng to delete").build();
		return Response.ok(result).build();
	}

	// get the complete list of locals
	@GET
	@Path("locals")
	@Produces(MediaType.TEXT_PLAIN)
	public Response getLocals() {
		Gson gson = new Gson();
		String Json = gson.toJson(Database.getAllLocals());
		if (Json == null)
			return Response.status(801).entity("Some Problem occurred").build();
		return Response.ok(Json, MediaType.APPLICATION_JSON).build();
	}

	// create a new local
	@POST
	@Path("locals")
	@Consumes(MediaType.TEXT_PLAIN)
	@Produces(MediaType.TEXT_PLAIN)
	public Response createNewLocal(String json) {
		Local local = new Gson().fromJson(json, Local.class);
		int result = Database.createNewLocal(local);
		if (result == -1)
			return Response.status(789).entity("This id is already used").build();
		else if (result == -2)
			return Response.status(801).entity("Some Problem occurred").build();
		return Response.ok().build();

	}
	
	@POST
	@Path("query")
	@Produces(MediaType.TEXT_PLAIN)
	public Response complexQuery(String parameters) throws SQLException {
		ComplexQuery query = new Gson().fromJson(parameters, ComplexQuery.class);
		Gson gson = new Gson();
		String Json = gson.toJson(Database.makeQuery(query));
		if (Json == null)
			return Response.status(801).entity("Some Problem occurred").build();

		return Response.ok(Json, MediaType.APPLICATION_JSON).build();
	}
}