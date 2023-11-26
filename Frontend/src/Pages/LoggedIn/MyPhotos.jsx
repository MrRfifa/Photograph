import { useEffect, useState } from "react";
import { UpdatesButton } from "../../Components/Buttons/CustomizedButtons";
import ImageService from "../../Services/User/ImageService";
import { FaSpinner } from "react-icons/fa";
import UserImageCard from "../../Components/Cards/UserImageCard";
import { UploadImageModal } from "../../Components/Modals";
import emptyImage from "../../assets/empty.png";
import StaticticService from "../../Services/User/StatisticService";

const MyPhotos = () => {
  const [images, setImages] = useState([]);
  const [openImageUploader, setOpenImageUploader] = useState(false);
  const [numberLikesReceived, setNumberLikesReceived] = useState(null);
  const [numberCommentsReceived, setNumberCommentsReceived] = useState(null);
  const [numberLikesDone, setNumberLikesDone] = useState(null);
  const [numberCommentsDone, setNumberCommentsDone] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [
          likesReceivedRes,
          commentsReceivedRes,
          likesDoneRes,
          commentsDoneRes,
        ] = await Promise.all([
          StaticticService.GetNumberLikesReceived(),
          StaticticService.GetNumberCommentsReceived(),
          StaticticService.GetNumberLikesDone(),
          StaticticService.GetNumberCommentsDone(),
        ]);

        setNumberLikesReceived(likesReceivedRes.message.message);
        setNumberCommentsReceived(commentsReceivedRes.message.message);
        setNumberLikesDone(likesDoneRes.message.message);
        setNumberCommentsDone(commentsDoneRes.message.message);
      } catch (error) {
        // Handle errors here
        console.error("Error fetching data:", error);
      }
    };

    fetchData();
  }, []);

  const handleImageUploader = () => setOpenImageUploader(true);
  const handleCloseImageUploader = () => setOpenImageUploader(false);
  useEffect(() => {
    const fetchData = async () => {
      try {
        const result = await ImageService.GetImagesByUser();
        if (result.success) {
          setImages(result.message);
        } else {
          console.error(result.error);
        }
      } catch (error) {
        console.error(error);
      }
    };

    fetchData();
  }, [images]);
  if (!images) {
    return (
      <div className="w-full h-full flex items-center justify-center ml-0 lg:ml-20">
        <FaSpinner className="text-blue-500 text-4xl animate-spin" />
      </div>
    );
  }

  return (
    <div className="w-full h-full flex flex-col text-white ml-10 lg:ml-20">
      <div className=" w-full p-4 flex flex-col lg:flex-row justify-between lg:justify-between">
        <div className="my-auto mx-auto">
          <UpdatesButton label="Upload image" onClick={handleImageUploader} />
        </div>

        <div className="mt-5 lg:mt-0 flex flex-col md:flex-row flex-wrap mx-auto">
          <span className="bg-transparent text-base cursor-default text-blue-800  font-medium me-2 px-2.5 py-2 lg:py-4 my-2 rounded border-2 hover:text-white hover:bg-blue-600">
            Received comments ({numberCommentsReceived})
          </span>
          <span className="bg-transparent text-base cursor-default text-red-600  font-medium me-2 px-2.5 py-2 lg:py-4 my-2 rounded border-2 hover:text-white hover:bg-red-600">
            Received likes ({numberLikesReceived})
          </span>
          <span className="bg-transparent text-base cursor-default text-yellow-800  font-medium me-2 px-2.5 py-2 lg:py-4 my-2 rounded border-2 hover:text-white hover:bg-yellow-800">
            Done comments ({numberCommentsDone})
          </span>
          <span className="bg-transparent text-base cursor-default text-green-800  font-medium me-2 px-2.5 py-2 lg:py-4 my-2 rounded border-2 hover:text-white hover:bg-green-600">
            Done likes ({numberLikesDone})
          </span>
        </div>

        <UploadImageModal
          open={openImageUploader}
          onClose={handleCloseImageUploader}
          key="imageuploadermodal"
        />
      </div>
      <div className="w-full">
        {images.length === 0 ? (
          <div>
            <img src={emptyImage} alt="empty" />
          </div>
        ) : (
          <div className="grid grid-cols-1 gap-5 md:grid-cols-2 lg:grid-cols-3">
            {images.map((image) => (
              <UserImageCard
                key={image.id} // Use a unique identifier as the key
                imageTitle={image.title}
                imageDescription={image.description}
                image={image.fileContentBase64}
                uploadDate={image.uploadDate}
                imageId={image.id}
              />
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default MyPhotos;
