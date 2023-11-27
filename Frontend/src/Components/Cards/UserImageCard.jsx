import PropTypes from "prop-types";
import { Link } from "react-router-dom";
import { RiDeleteBin6Line } from "react-icons/ri";
import ImageService from "../../Services/User/ImageService";
import { useEffect, useState } from "react";
import { ImSpinner9 } from "react-icons/im";

const UserImageCard = ({
  imageTitle,
  imageDescription,
  image,
  uploadDate,
  imageId,
  privatePage,
}) => {
  const [isDeleting, setIsDeleting] = useState(false);
  const [imageBeingDeletedId, setImageBeingDeletedId] = useState(null);

  const handleDeleteImage = async (imageId) => {
    try {
      setImageBeingDeletedId(imageId);
      setIsDeleting(true);
      await ImageService.DeleteImage(imageId);
    } catch (error) {
      console.error("Error deleting comment:", error);
    } finally {
      setIsDeleting(false);
      setImageBeingDeletedId(null);
    }
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        await ImageService.GetImagesByUser();
      } catch (error) {
        console.error(error);
      }
    };

    fetchData();
  }, []);

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
        {imageTitle.length > 10 ? `${imageTitle.slice(0, 10)}...` : imageTitle}
      </div>
      <div className="w-full flex flex-row justify-between">
        <Link
          to={`/image/${imageId}`}
          className="bg-sky-700 font-extrabold p-2 px-6 rounded-xl hover:bg-sky-500 transition-colors"
        >
          See details
        </Link>
        {privatePage && (
          <>
            {isDeleting && imageId === imageBeingDeletedId ? (
              <ImSpinner9 className="text-blue-500 animate-spin" size={25} />
            ) : (
              <RiDeleteBin6Line
                color="red"
                size={25}
                className="hover:scale-125 cursor-pointer mt-1.5"
                onClick={() => {
                  handleDeleteImage(imageId);
                }}
              />
            )}
          </>
        )}
      </div>
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
  privatePage: PropTypes.bool.isRequired,
};
