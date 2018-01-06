package rest;

import java.sql.DriverManager;
import java.sql.SQLException;

import javax.servlet.ServletContextEvent;
import javax.servlet.ServletContextListener;
import javax.servlet.annotation.WebListener;
import java.sql.Connection;

@WebListener
public class MyListener implements ServletContextListener {

	public MyListener() {
	}

	@Override
	public void contextDestroyed(ServletContextEvent arg0) {
		com.mysql.jdbc.AbandonedConnectionCleanupThread.checkedShutdown();
		try {
			java.sql.Driver mySqlDriver = DriverManager.getDriver("jdbc:mysql://localhost:3306/paldb");
			DriverManager.deregisterDriver(mySqlDriver);
		} catch (SQLException ex) {
			System.out.println("Could not deregister driver:".concat(ex.getMessage()));
		}
	}

	@Override
	public void contextInitialized(ServletContextEvent arg0) {
	}

}
