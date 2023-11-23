import PropTypes from "prop-types";
import Sidebar from "./Sidebar";

function SidebarLayout({ children }) {
  return (
    <div className="flex">
      <Sidebar />
      <main className="max-w-5xl flex-1 mx-auto py-2 mr-16 lg:mr-48">
        {children}
      </main>
    </div>
  );
}

export default SidebarLayout;

SidebarLayout.propTypes = {
  children: PropTypes.node.isRequired,
};
