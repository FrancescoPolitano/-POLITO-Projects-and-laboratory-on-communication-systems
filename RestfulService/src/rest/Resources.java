package rest;

import java.sql.SQLException;
import java.util.ArrayList;

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
public class Resources {
	Database database;

	public Resources() {
		database = Database.getInstance();
	}

	// returns all the employes
	@GET
	@Path("users/employees")
	@Produces(MediaType.TEXT_PLAIN)
	public Response sendAllEmployes() {
		if (!database.isAdminLogged())
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();
		Gson gson = new Gson();
		// EmployeeRequestClass.testing();
		String Json = gson.toJson(database.getAllEmployes());
		if (Json == null)
			return Response.status(Constants.status_generic_error).entity(Constants.generic_error).build();
		return Response.ok(Json, MediaType.APPLICATION_JSON).build();

	}

	@GET
	@Path("users/visitors")
	@Produces(MediaType.TEXT_PLAIN)
	public Response sendAllVisitors() {
		if (!database.isAdminLogged())
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();
		Gson gson = new Gson();
		String Json = gson.toJson(database.getAllVisitors());
		if (Json == null)
			return Response.status(Constants.status_generic_error).entity(Constants.generic_error).build();
		return Response.ok(Json, MediaType.APPLICATION_JSON).build();

	}

	// returns the employee related to the chosen id
	@GET
	@Path("users/{id}")
	@Produces(MediaType.TEXT_PLAIN)
	public Response sendEmployee(@PathParam("id") String id) throws SQLException {
		if (!database.isAdminLogged())
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();
		Gson gson = new Gson();
		Employee temp = database.getEmployee(id);
		if (temp == null)
			return Response.status(Constants.status_not_found).entity(Constants.not_found).build();
		String Json = gson.toJson(temp);
		return Response.ok(Json, MediaType.APPLICATION_JSON).build();
	}

	// create a new employee and return the user and the QRcode (not working)
	@POST
	@Path("users/employees")
	@Produces(MediaType.TEXT_PLAIN)
	public Response newEmployee(String employee) {
		if (!database.isAdminLogged())
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();
		EmployeeRequestClass temp = new Gson().fromJson(employee, EmployeeRequestClass.class);
		if (temp == null)
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();
		if (temp.getEmployee() == null)
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();

		EmployeeResponseClass newEmployee = database.createEmployee(temp);
		if (newEmployee == null)
			return Response.status(Constants.status_generic_error).entity(Constants.generic_error).build();
		Gson gson = new Gson();
		String Json = gson.toJson(newEmployee);
		return Response.ok(Json).build();
	}

	// return if the code is valid or not for the selected locals
	@GET
	@Path("auth/{local}/{code}")
	@Produces(MediaType.TEXT_PLAIN)
	public Response getAccess(@PathParam("local") String door, @PathParam("code") String code) {
		if (!database.isAdminLogged())
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();
		return Response.ok(database.isAuth(door, code)).build();
	}

	// create a temporary user and grant him the lowest level of permission, returns
	// the user as a json
	@POST
	@Path("users/visitors")
	public Response newVisitor(String visitor) {
		if (!database.isAdminLogged())
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();
		rest.Visitor temp = new Gson().fromJson(visitor, rest.Visitor.class);
		if (temp == null)
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();
		String code = database.createVisitor(temp);
		if (code == null)
			return Response.status(Constants.status_generic_error).entity(Constants.generic_error).build();
		return Response.ok(code).build();
	}

	// revoke a user QRCode and obtain a new one
	// MUST FIX THE ERROR MANAGMENT
	@POST
	@Path("users/new_code")
	@Produces(MediaType.TEXT_PLAIN)
	public Response getNewCode(String id) {
		if (!database.isAdminLogged())
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();
		if (id.isEmpty())
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();
		String code = database.newCode(id);
		if (code == null)
			return Response.status(Constants.status_generic_error).entity(Constants.generic_error).build();
		return Response.ok(code).build();

	}

	// delete an user
	@DELETE
	@Path("users/{id}")
	public Response deleteEmployee(@PathParam("id") String id) {
		if (!database.isAdminLogged())
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();
		boolean result = database.deleteEmployee(id);
		if (!result)
			return Response.status(Constants.status_not_found).entity(Constants.not_found).build();
		return Response.ok(result).build();
	}

	// get the complete list of locals
	@GET
	@Path("locals")
	@Produces(MediaType.TEXT_PLAIN)
	public Response getLocals() {
		if (!database.isAdminLogged())
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();
		Gson gson = new Gson();
		ArrayList<Local> locals = database.getAllLocals();
		if (locals == null)
			return Response.status(Constants.status_generic_error).entity(Constants.generic_error).build();
		String Json = gson.toJson(locals);
		return Response.ok(Json, MediaType.APPLICATION_JSON).build();
	}

	// create a new local
	@POST
	@Path("locals")
	@Produces(MediaType.TEXT_PLAIN)
	public Response createNewLocal(String json) {
		if (!database.isAdminLogged())
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();
		Local local = new Gson().fromJson(json, Local.class);
		if (local == null)
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();
		int result = database.createNewLocal(local);
		if (result == -1)
			return Response.status(Constants.status_existing_key).entity(Constants.existing_key).build();
		else if (result == -2)
			return Response.status(Constants.status_generic_error).entity(Constants.generic_error).build();
		return Response.ok(new Gson().toJson(local.getIdLocal())).build();
	}

	// gestire meglio
	@POST
	@Path("query")
	@Produces(MediaType.TEXT_PLAIN)
	public Response complexQuery(String parameters) throws SQLException {
		if (!database.isAdminLogged())
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();
		ComplexQuery query = new Gson().fromJson(parameters, ComplexQuery.class);
		if (query == null)
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();
		Gson gson = new Gson();
		ArrayList<Access> temp = database.makeQuery(query);

		if (temp == null)
			return Response.status(Constants.status_generic_error).entity(Constants.generic_error).build();
		String Json = gson.toJson(temp);
		return Response.ok(Json, MediaType.APPLICATION_JSON).build();
	}

	// login Admin
	@POST
	@Path("login")
	@Produces(MediaType.TEXT_PLAIN)
	public Response adminLogin(String json) {
		LoginData lg = new Gson().fromJson(json, LoginData.class);
		if (lg.getPassword() == null || lg.getUsername() == null || lg == null)
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();
		if (database.login(lg))
			return Response.ok().build();
		return Response.status(Constants.status_generic_error).entity(Constants.generic_error).build();
	}

	// logout Admin
	@GET
	@Path("logout")
	@Produces(MediaType.TEXT_PLAIN)
	public Response adminLogout() {
		if (!database.isAdminLogged())
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();
		Database.getInstance().setAdminLogged(false);
		return Response.ok().build();
	}
}