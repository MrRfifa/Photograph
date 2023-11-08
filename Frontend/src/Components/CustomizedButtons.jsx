import { Link } from "react-router-dom";
import PropTypes from "prop-types";

export const LabelDestinationLinkButton = ({ nav, label, destination }) => {
  return (
    <Link
      to={destination}
      className={`${
        nav
          ? "w-[100px] bg-[#231942] h-[30px] flex items-center justify-center rounded-xl cursor-pointer relative overflow-hidden transition-all duration-500 ease-in-out shadow-md hover:scale-105 hover:shadow-lg before:absolute before:top-0 before:-left-full before:w-full before:h-full before:bg-gradient-to-r before:from-[#9F86C0] before:to-[#E0B1CB] before:transition-all before:duration-500 before:ease-in-out before:z-[-1] before:rounded-xl hover:before:left-0 text-[#fff]"
          : "text-center  bg-[#E0B1CB] w-[200px] rounded-md font-medium my-6 mx-auto py-3 text-black transition duration-300 ease-in-out transform hover:scale-105 hover:z-10 hover:bg-[#C687A5] hover:text-white"
      }`}
    >
      {label}
    </Link>
  );
};

export const UpdatesButton = ({ label, onClick, type }) => {
  return (
    <button
      type={type}
      onClick={onClick}
      className=" relative px-8 py-2 rounded-md bg-white isolation-auto z-0 border-2 border-[#7209b7] text-[#7209b7]
      hover:text-white before:absolute before:w-full before:transition-all before:duration-700 before:hover:w-full before:-right-full before:hover:right-0 before:rounded-full  before:bg-[#7209b7] before:-z-10  before:aspect-square before:hover:scale-150 overflow-hidden before:hover:duration-700"
    >
      {label}
    </button>
  );
};

export const DangerButton = ({ label }) => {
  return (
    <button
      className="relative px-8 py-2 rounded-md bg-white text-red-600 isolation-auto z-0 border-2 border-red-600
      before:absolute before:w-full before:transition-all before:duration-700 before:hover:w-full before:-right-full before:hover:right-0 before:rounded-full  before:bg-red-600 before:text-white before:-z-10  before:aspect-square before:hover:scale-150 overflow-hidden before:hover:duration-700
      hover:text-white"
    >
      {label}
    </button>
  );
};

LabelDestinationLinkButton.propTypes = {
  label: PropTypes.string.isRequired,
  destination: PropTypes.string.isRequired,
  nav: PropTypes.bool,
};

UpdatesButton.propTypes = {
  label: PropTypes.string.isRequired,
  onClick: PropTypes.func,
  type: PropTypes.string,
};
DangerButton.propTypes = {
  label: PropTypes.string.isRequired,
};
