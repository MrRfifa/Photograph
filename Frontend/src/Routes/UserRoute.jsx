import { Route, Routes, Navigate } from "react-router-dom";
import Home from "../Pages/LoggedIn/Home";
import MyPhotos from "../Pages/LoggedIn/MyPhotos";
import Settings from "../Pages/LoggedIn/Settings";
import SidebarLayout from "../Layout/Sidebar/SidebarLayout";
import ImageDetails from "../Pages/LoggedIn/ImageDetails";

const AdminRouter = () => {
  return (
    <SidebarLayout>
      <Routes>
        <Route path="/home" exact element={<Home />} />
        <Route path="/my-photos" exact element={<MyPhotos />} />
        <Route path="/settings" exact element={<Settings />} />
        <Route exact path="/image/:id" element={<ImageDetails />} />
        <Route path="*" element={<Navigate to="/home" />} />
      </Routes>
    </SidebarLayout>
  );
};

export default AdminRouter;
