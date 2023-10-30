import { Route, Routes, Navigate } from "react-router-dom";
import Home from "../Pages/LoggedIn/Home";
import MyPhotos from "../Pages/LoggedIn/MyPhotos";
import Settings from "../Pages/LoggedIn/Settings";
//import RootLayout from "../Layouts/RootLayout";

const AdminRouter = () => {
  return (
    <Routes>
      <Route path="/home" exact element={<Home />} />
      <Route path="/my-photos" exact element={<MyPhotos />} />
      <Route path="/settings" exact element={<Settings />} />
      <Route path="*" element={<Navigate to="/home" />} />
    </Routes>
    // <RootLayout>
    // </RootLayout>
  );
};

export default AdminRouter;
