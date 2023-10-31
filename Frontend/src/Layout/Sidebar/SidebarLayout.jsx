import PropTypes from "prop-types";
import Sidebar from "./Sidebar";

function SidebarLayout({ children }) {
  return (
    <div className="flex gap-5 ">
      <Sidebar />
      <main className="max-w-5xl flex-1 mx-auto py-4">{children}</main>
    </div>
  );
}

export default SidebarLayout;

SidebarLayout.propTypes = {
  children: PropTypes.node.isRequired,
};
