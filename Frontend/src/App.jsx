import { BrowserRouter as Router } from "react-router-dom";
import AuthRoute from "./Routes/AuthRoute";
import UserRoute from "./Routes/UserRoute";
import AuthVerifyService from "./Services/Auth/AuthVerifyService";
import { AuthContextProvider } from "./Context/AuthContext";

function App() {
  const authStatus = AuthVerifyService.AuthVerify();

  if (authStatus === 0) {
    return (
      <Router>
        <AuthRoute />
      </Router>
    );
  }
  if (authStatus === 1) {
    return (
      <AuthContextProvider>
        <Router>
          <UserRoute />
        </Router>
      </AuthContextProvider>
    );
  }

  return null; // Return something in case neither of the conditions is met
}

export default App;
