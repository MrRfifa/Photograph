import PropTypes from "prop-types";
import { Link } from "react-router-dom";
//import ImageDetails from "../../Pages/LoggedIn/ImageDetails";

const UserImageCard = ({
  imageTitle,
  imageDescription,
  image,
  uploadDate,
  imageId,
}) => {
  return (
    <div className="w-60 h-80 bg-[#065a60] rounded-3xl text-neutral-300 p-4 flex flex-col items-start justify-center gap-3 hover:bg-gray-900 hover:shadow-2xl hover:shadow-[#065a60] transition-shadow">
      <div>
        <span className="ml-24">{uploadDate}</span>
        <div className="w-52 h-40 bg-sky-300 rounded-2xl">
          <img
            className="w-full h-full"
            src={`data:image/png;base64,${image}`}
          />
        </div>
      </div>
      <div className="">
        <p className="font-extrabold">{imageDescription}</p>
        <p className="">{imageTitle}</p>
      </div>
      <Link
        to={`/image/${imageId}`}
        className="bg-sky-700 font-extrabold p-2 px-6 rounded-xl hover:bg-sky-500 transition-colors"
      >
        See details
      </Link>
    </div>
  );
};

export default UserImageCard;

UserImageCard.propTypes = {
  imageTitle: PropTypes.string,
  imageDescription: PropTypes.string,
  image: PropTypes.string,
  uploadDate: PropTypes.string,
  imageId: PropTypes.number,
};
