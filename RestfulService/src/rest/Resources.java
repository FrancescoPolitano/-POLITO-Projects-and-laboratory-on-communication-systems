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
import com.google.gson.GsonBuilder;
import com.google.gson.internal.LinkedTreeMap;

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
	public Response sendEmployee(@PathParam("id") String id) throws SQLException {
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
	public Response newEmployee(String request) {
		AuthenticatedRequest req = new Gson().fromJson(request, AuthenticatedRequest.class);
		if (!Database.tokens.containsValue(req.Token))
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();
		@SuppressWarnings("unchecked")
		EmployeeRequestClass temp = new Gson().fromJson(
				new Gson().toJson(((LinkedTreeMap<String, Object>) req.getBody())), EmployeeRequestClass.class);
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
		return Response.ok(database.isAuth(door, code)).build();
	}

	// create a temporary user and grant him the lowest level of permission, returns
	// the user as a json
	@POST
	@Path("users/visitors")
	public Response newVisitor(String request) {
		AuthenticatedRequest req = new Gson().fromJson(request, AuthenticatedRequest.class);
		if (!Database.tokens.containsValue(req.Token))
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();
		@SuppressWarnings("unchecked")
		rest.Visitor temp = new Gson().fromJson(new Gson().toJson(((LinkedTreeMap<String, Object>) req.getBody())),
				rest.Visitor.class);
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
	public Response getNewCode(String request) {
		AuthenticatedRequest req = new Gson().fromJson(request, AuthenticatedRequest.class);
		if (!Database.tokens.containsValue(req.Token))
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();
		@SuppressWarnings("unchecked")
		String id = new Gson().fromJson(new Gson().toJson(((LinkedTreeMap<String, Object>) req.getBody())),
				String.class);
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
	public Response deleteEmployee(@PathParam("id") String request) {
		AuthenticatedRequest req = new Gson().fromJson(request, AuthenticatedRequest.class);
		if (!Database.tokens.containsValue(req.Token))
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();
		@SuppressWarnings("unchecked")
		String id = new Gson().fromJson(new Gson().toJson(((LinkedTreeMap<String, Object>) req.getBody())),
				String.class);
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
		Gson gson = new Gson();
		ArrayList<Local> locals = database.getAllLocals();
		System.out.println(Utils.hashString("admin"));
		if (locals == null)
			return Response.status(Constants.status_generic_error).entity(Constants.generic_error).build();
		String Json = gson.toJson(locals);
		return Response.ok(Json, MediaType.APPLICATION_JSON).build();
	}

	// create a new local
	@POST
	@Path("locals")
	@Produces(MediaType.TEXT_PLAIN)
	public Response createNewLocal(String request) {
		AuthenticatedRequest req = new Gson().fromJson(request, AuthenticatedRequest.class);
		if (!Database.tokens.containsValue(req.Token))
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();
		@SuppressWarnings("unchecked")
		Local local = new Gson().fromJson(new Gson().toJson(((LinkedTreeMap<String, Object>) req.getBody())),
				Local.class);
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
	public Response complexQuery(String request) {
		AuthenticatedRequest req = new Gson().fromJson(request, AuthenticatedRequest.class);
		if (!Database.tokens.containsValue(req.Token))
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();
		@SuppressWarnings("unchecked")
		ComplexQuery query = new Gson().fromJson(new Gson().toJson(((LinkedTreeMap<String, Object>) req.getBody())),
				ComplexQuery.class);
		Gson gson = new GsonBuilder().setDateFormat("yyyy-MM-dd'T'HH:mm:ss").create();
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
		
		LoginData lg = new Gson().fromJson(request, LoginData.class);
		if (lg.getPassword() == null || lg.getUsername() == null || lg == null)
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();
		String token = database.login(lg);
		if (token != null)
			return Response.ok(token, MediaType.TEXT_PLAIN).build();
		return Response.status(Constants.status_generic_error).entity(Constants.generic_error).build();
	}
	
	// login Admin
	@POST
	@Path("logout")
	@Produces(MediaType.TEXT_PLAIN)
	public Response adminLogout(String request) {
		
		AuthenticatedRequest req = new Gson().fromJson(request, AuthenticatedRequest.class);
		if (!Database.tokens.containsValue(req.Token))
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();
		String user = new Gson().fromJson(new Gson().toJson(((LinkedTreeMap<String, Object>) req.getBody())),
				String.class);
		if(Database.tokens.remove(user,req.Token))
			return Response.ok().build();
		return Response.status(Constants.status_generic_error).entity(Constants.generic_error).build();
	}

	// change authLevel  
	@POST
	@Path("users/authLevel")
	@Produces(MediaType.TEXT_PLAIN)
	public Response changeAuthLevel(String request) {
		System.out.println("wdicbbceurberuvb");
		AuthenticatedRequest req = new Gson().fromJson(request, AuthenticatedRequest.class);
		if (!Database.tokens.containsValue(req.Token))
			return Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();
		@SuppressWarnings("unchecked")
		AuthLevel al = new Gson().fromJson(new Gson().toJson(((LinkedTreeMap<String, Object>) req.getBody())),
				AuthLevel.class);
		if (al.getAuthLevel() == null || al == null)
			return Response.status(Constants.status_invalid_input).entity(Constants.invalid_input).build();
		int result = database.changeAuthLevel(al);
		if (result == -1)
			return Response.status(Constants.status_generic_error).entity(Constants.generic_error).build();
		else if (result == 0)
			return Response.status(Constants.status_not_found).entity(Constants.not_found).build();
		else
			return Response.ok().build();
	}

	// logout Admin ---->sbagliata ora è una post
	// @GET
	// @Path("logout")
	// @Produces(MediaType.TEXT_PLAIN)
	// public Response adminLogout() {
	// if (!database.isAdminLogged())
	// return
	// Response.status(Constants.status_access_denied).entity(Constants.access_denied).build();
	// Database.getInstance().setAdminLogged(false);
	// return Response.ok().build();
	// }
}