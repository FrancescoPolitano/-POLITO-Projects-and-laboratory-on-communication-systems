package rest;
 
public class Errors {
    public static final String generic_error = "Fatal error: try later.";
    public static final String not_found = "We can't find what you want.";
    public static final String invalid_input = "Your input is invalid.";
    public static final String existing_key = "The selected Id is already in use.";
    public static final int status_generic_error = 801;
    public static final int status_not_found = 404;
    public static final int status_invalid_input = 802;
    public static final int status_existing_key = 803;
}