import { useEffect, useState } from "react";
import { UpdatesButton } from "../../Components/CustomizedButtons";
import ImageService from "../../Services/User/ImageService";
import { FaSpinner } from "react-icons/fa";
import UserImageCard from "../../Components/Cards/UserImageCard";
import { UploadImageModal } from "../../Components/Modals";
import emptyImage from "../../assets/empty.png";

const MyPhotos = () => {
  const [images, setImages] = useState([]);
  const [openImageUploader, setOpenImageUploader] = useState(false);

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
      <div className="w-full h-full flex items-center justify-center">
        <FaSpinner className="text-blue-500 text-4xl animate-spin" />
      </div>
    );
  }

  return (
    <div className="w-full h-full flex flex-col text-white  md:ml-[10rem]">
      <div className="w-full p-4 flex mx-auto">
        <UpdatesButton label="Upload image" onClick={handleImageUploader} />
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
                key={image.id}
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
