package rest;

import java.sql.SQLException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;

import javax.ws.rs.CookieParam;
import javax.ws.rs.DELETE;
import javax.ws.rs.GET;
import javax.ws.rs.POST;
import javax.ws.rs.Path;
import javax.ws.rs.PathParam;
import javax.ws.rs.Produces;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.NewCookie;
import javax.ws.rs.core.Response;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

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
		Gson gson = new Gson();
		String Json = gson.toJson(database.getAllEmployes());
		if (Json == null)
			return Response.status(Constants.status_generic_error).entity(Constants.generic_error).build();
		return Response.ok(Json, MediaType.APPLICATION_JSON).build();
	}

	// return all the visitors
	@GET
	@Path("users/visitors")
	@Produces(MediaType.TEXT_PLAIN)
	public Response sendAllVisitors() {
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
	public Response sendEmployee(@PathParam("id") String request) throws SQLException {
		if (request.trim().isEmpty())
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();
		Gson gson = new Gson();
		String id = gson.fromJson(request, String.class);
		Employee temp = database.getEmployee(id);
		if (temp == null)
			return Response.status(Constants.status_not_found).entity(Constants.not_found).build();
		String Json = gson.toJson(temp);
		return Response.ok(Json, MediaType.APPLICATION_JSON).build();
	}

	// create a new employee and return the user and the QRcode
	@POST
	@Path("users/employees")
	@Produces(MediaType.TEXT_PLAIN)
	public Response newEmployee(String request, @CookieParam("Token") String token) {
		if (request.trim().isEmpty())
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();

		if (!database.isValidToken(token))
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();
		Gson gson = new Gson();
		EmployeeRequestClass temp = gson.fromJson(request, EmployeeRequestClass.class);
		if (temp == null)
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();

		if (temp.getEmployee() == null)
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();

		EmployeeResponseClass newEmployee = database.createEmployee(temp);
		if (newEmployee == null)
			return Response.status(Constants.status_generic_error).entity(Constants.generic_error).build();

		String Json = gson.toJson(newEmployee);
		return Response.ok(Json).build();
	}

	// modify some fields of an existing employee
	@POST
	@Path("users/modify")
	public Response modifyEmployee(String request, @CookieParam("Token") String token) {
		if (request.trim().isEmpty())
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();
		if (!database.isValidToken(token))
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();

		Gson gson = new Gson();
		Employee newEmployee = gson.fromJson(request, Employee.class);
		if (newEmployee == null || newEmployee.getName() == null || newEmployee.getSurname() == null
				|| newEmployee.getEmail() == null || newEmployee.getAuthLevel() == null
				|| newEmployee.getSerial() == null)
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();

		boolean result = database.modifyEmployee(newEmployee);
		if (!result)
			return Response.status(Constants.status_generic_error).entity(Constants.generic_error).build();
		else
			return Response.ok().build();
	}

	// return if the code is valid or not for the selected locals
	@GET
	@Path("auth/{local}/{code}")
	@Produces(MediaType.TEXT_PLAIN)
	public Response getAccess(@PathParam("local") String door, @PathParam("code") String code) {
		return Response.ok(database.isAuth(door, code)).build();
	}

	// email confirmation link
	@GET
	@Path("confirm/{code}")
	@Produces(MediaType.TEXT_PLAIN)
	public Response confirm(@PathParam("code") String code) {
		database.confirmEmail(code);
		return Response.ok().build();
	}

	// create a temporary user (visitor)
	@POST
	@Path("users/visitors")
	public Response newVisitor(String request, @CookieParam("Token") String token) {

		if (request.trim().isEmpty())
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();

		if (!database.isValidToken(token))
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();

		rest.Visitor temp = new Gson().fromJson(request, rest.Visitor.class);
		if (temp == null)
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();
		VisitorResponseClass vr = database.createVisitor(temp);
		if (vr == null)
			return Response.status(Constants.status_generic_error).entity(Constants.generic_error).build();
		return Response.ok(vr).build();
	}

	// revoke a user QRCode and obtain a new one
	@POST
	@Path("users/new_code")
	@Produces(MediaType.TEXT_PLAIN)
	public Response getNewCode(String request, @CookieParam("Token") String token) {
		if (request.trim().isEmpty())
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();

		if (!database.isValidToken(token))
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();

		String code = database.newCode(new Gson().fromJson(request, String.class));
		if (code == null)
			return Response.status(Constants.status_generic_error).entity(Constants.generic_error).build();
		return Response.ok(code).build();
	}

	// delete an user
	@DELETE
	@Path("users/{id}")
	public Response deleteEmployee(@PathParam("id") String request, @CookieParam("Token") String token) {
		if (request.trim().isEmpty())
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();

		if (!database.isValidToken(token))
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();

		boolean result = database.deleteEmployee(new Gson().fromJson(request, String.class));
		if (!result)
			return Response.status(Constants.status_not_found).entity(Constants.not_found).build();
		return Response.ok(result).build();
	}

	// get the complete list of locals
	@GET
	@Path("locals")
	@Produces(MediaType.TEXT_PLAIN)
	public Response getLocals() {
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
	public Response createNewLocal(String request, @CookieParam("Token") String token) {
		if (request.trim().isEmpty())
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();

		if (!database.isValidToken(token))
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();

		Local local = new Gson().fromJson(request, Local.class);
		if (local == null)
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();
		int result = database.createNewLocal(local);
		if (result == -1)
			return Response.status(Constants.status_existing_key).entity(Constants.existing_key).build();
		else if (result == -2)
			return Response.status(Constants.status_generic_error).entity(Constants.generic_error).build();
		return Response.ok(new Gson().toJson(local.getIdLocal())).build();
	}

	// return parametrized query
	@POST
	@Path("query")
	@Produces(MediaType.TEXT_PLAIN)
	public Response complexQuery(String request, @CookieParam("Token") String token) {
		if (request.trim().isEmpty())
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();

		if (!database.isValidToken(token))
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();

		Gson gson = new GsonBuilder().setDateFormat("yyyy-MM-dd'T'HH:mm:ss").create();
		ComplexQuery query = gson.fromJson(request, ComplexQuery.class);

		if (query == null)
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();
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
	public Response adminLogin(String request) {
		if (request.trim().isEmpty())
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();

		database.deleteOldTokens();
		LoginData lg = new Gson().fromJson(request, LoginData.class);

		if (lg.getPassword() == null || lg.getUsername() == null || lg == null)
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();

		Date tomorrow = new Date(new Date().getTime() + (1000 * 60 * 60 * 24));
		SimpleDateFormat format = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
		String expiryDate = format.format(tomorrow);

		String token = database.login(lg, expiryDate);
		if (token == null)
			return Response.status(Constants.status_generic_error).entity(Constants.generic_error).build();

		NewCookie tokenCookie = new NewCookie("Token", token);
		return Response.ok(token).cookie(tokenCookie).build();
	}

	// logout Admin
	@GET
	@Path("logout")
	@Produces(MediaType.TEXT_PLAIN)
	public Response adminLogout(@CookieParam("Token") String token) {
		if (token.isEmpty() || token == null)
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();
		boolean result = database.logout(token);
		if (!result)
			return Response.status(Constants.status_generic_error).entity(Constants.generic_error).build();
		else
			return Response.ok().build();
	}

	// Revoke authorization level
	@POST
	@Path("users/block")
	public Response blockUser(String request, @CookieParam("Token") String token) {
		if (request.trim().isEmpty())
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();

		if (!database.isValidToken(token))
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();

		int serial = -1;
		try {
			serial = Integer.parseInt(new Gson().fromJson(request, String.class));
		} catch (NumberFormatException e) {
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();
		}

		if (database.blockUsers(serial) == -1)
			return Response.status(Constants.status_generic_error).entity(Constants.generic_error).build();
		else if (database.blockUsers(serial) == 0)
			return Response.status(Constants.status_not_found).entity(Constants.not_found).build();
		else
			return Response.ok().build();
	}
}