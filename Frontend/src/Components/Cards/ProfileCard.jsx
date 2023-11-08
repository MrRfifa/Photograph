import PropTypes from "prop-types";
import manSvg from "../../assets/Genders/male-svg.svg";
import womanSvg from "../../assets/Genders/female-svg.svg";

const ProfileCard = ({ firstname, lastname, email, gender, dateOfBirth }) => {
  const isGenderMale = gender.toLowerCase() === "male"; // Convert to lowercase for comparison
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 justify-center items-center uppercase mt-4 mx-auto max-w-[600px] w-full rounded-lg hover:shadow-lg transition-transform duration-300 p-6 bg-gradient-to-br from-[#5A189A] to-[#E0AAFF] text-white">
      <div className="flex flex-col items-center md:items-start text-center md:text-left">
        <h4 className="text-2xl font-semibold mb-2">
          {firstname} {lastname}
        </h4>
        <p className="text-lg">{email}</p>
        <p className="text-sm text-slate-300 mt-2">Gender: {gender}</p>
        <p className="text-sm text-slate-300 mt-2">
          Date Of Birth: {dateOfBirth}
        </p>
      </div>
      <div className="flex justify-center md:justify-end mt-4 md:mt-0">
        <img
          src={isGenderMale ? manSvg : womanSvg}
          className="w-[75%] md:w-[100%]"
          alt=""
        />
      </div>
    </div>
  );
};

export default ProfileCard;

ProfileCard.propTypes = {
  firstname: PropTypes.string,
  lastname: PropTypes.string,
  email: PropTypes.string,
  gender: PropTypes.string.isRequired,
  dateOfBirth: PropTypes.string.isRequired,
};
