import PropTypes from "prop-types";
import Sidebar from "./Sidebar";

function SidebarLayout({ children }) {
  return (
    <div className="flex">
      <Sidebar />
      <main className="max-w-5xl flex-1 mx-auto py-2 px-4">{children}</main>
    </div>
  );
}

export default SidebarLayout;

SidebarLayout.propTypes = {
  children: PropTypes.node.isRequired,
};
