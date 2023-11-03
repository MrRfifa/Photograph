import PropTypes from "prop-types";
import { Toaster } from "react-hot-toast";

const ModalComponent = ({ open, onClose, children }) => {
  return (
    <div
      onClick={onClose}
      className={`fixed inset-0 flex justify-center z-50 items-center transition-colors
        ${open ? "visible bg-black/20" : "invisible"} 
      `}
    >
      <Toaster />
      <div
        onClick={(e) => e.stopPropagation()}
        className={`bg-[#240046] rounded-xl shadow p-6 transition-all max-w-[500px]
          ${open ? "scale-100 opacity-100" : "scale-125 opacity-0"}
          `}
      >
        <button
          onClick={onClose}
          className="absolute top-2 right-2 p-1 rounded-lg text-gray-400 bg-white hover:bg-gray-400 hover:text-gray-600"
        >
          X
        </button>
        {children}
      </div>
    </div>
  );
};

export default ModalComponent;

ModalComponent.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func,
  children: PropTypes.node.isRequired,
};
