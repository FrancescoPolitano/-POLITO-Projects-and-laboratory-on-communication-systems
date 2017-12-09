package rest;

import java.sql.SQLException;

import javax.ws.rs.Consumes;
import javax.ws.rs.DELETE;
import javax.ws.rs.GET;
import javax.ws.rs.POST;
import javax.ws.rs.Path;
import javax.ws.rs.PathParam;
import javax.ws.rs.Produces;
import javax.ws.rs.client.Entity;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;

import com.google.gson.Gson;

import javassist.compiler.ast.Visitor;

//Sets the bae url
@Path("/resources")
public class Resources {
	public Resources() throws SQLException {
		Database.init();
	}

	// returns all the employes
	@GET
	@Path("users")
	@Produces(MediaType.TEXT_PLAIN)
	public Response sendAllEmployes() {
		Gson gson= new Gson();
		 String Json = gson.toJson(Database.getAllEmployes());
	        if (Json == null) return Response.status(801).entity("We're in big trubles").build();
	        return Response.ok(Json, MediaType.APPLICATION_JSON).build();		
        
	}

	// returns the employee related to the chosen id
	@GET
	@Path("users/{id}")
	@Produces(MediaType.TEXT_PLAIN)
	public Response sendEmployee(@PathParam("id") String id) throws SQLException {
		Gson gson= new Gson();
		 String Json = gson.toJson(Database.getEmployee(id));
	        if (Json == null) return Response.status(404).entity("The resource doesn't exist").build();
	        return Response.ok(Json, MediaType.APPLICATION_JSON).build();	
	}

	
	// return if the code is valid or not for the selected locals
	@GET
	@Path("auth/{local}/{code}")	
	@Produces(MediaType.TEXT_PLAIN)
	public Response getAccess(@PathParam("local") String door,@PathParam("code") String code) {
		
        return Response.ok(Database.isAuth(door,code)).build();	
		}
	
	// create a temporary user and grant him the lowest level of permission, returns the user as a json
	@POST
	@Path("auth/visitor")	
	public Response newVisitor(String visitor) {
		rest.Visitor temp= new Gson().fromJson(visitor, rest.Visitor.class);
//		rest.Visitor temp= new rest.Visitor("ciro", "dimarzio", "estorsione", "10:10:10");
		//TODO return the qrcode image
		return Response.ok(Database.createVisitor(temp)).build();	
		}
	
	
	// revoke a user QRCode and obtain a new one
		@POST
		@Path("users/new_code")	
		public Response codeRestoration(String id) {
					
			//TODO return the qrcode image
			return Response.ok(Database.newCode(id)	).build();	
			}
		
		
	// delete an user
		@DELETE
		@Path("users/{id}")	
		public Response deleteEmployee(@PathParam("id") String id) {
			boolean result=Database.deleteEmployee(id);
			return Response.ok(result).build();	
			}
		
		//get the complete list of locals
		@GET
		@Path("locals")	
		@Produces(MediaType.TEXT_PLAIN)
		public Response getLocals() {
			Gson gson= new Gson();
			String Json = gson.toJson(Database.getAllLocals());
			if (Json == null)
				return Response.status(801).entity("Some Problem occurred").build();
			return Response.ok(Json, MediaType.APPLICATION_JSON).build();	
		}

		//create a new local
		@POST
		@Path("locals")	
		@Consumes(MediaType.TEXT_PLAIN)
		@Produces(MediaType.TEXT_PLAIN)
		public Response createNewLocal(String json) {
			Local local = new Gson().fromJson(json,Local.class);
			int result = Database.createNewLocal(local);
			if(result == -1)
				return Response.status(789).entity("This id is already used").build();
			else if(result == -2)
				return Response.status(801).entity("Some Problem occurred").build();
			return Response.ok().build();	
		
		}
}