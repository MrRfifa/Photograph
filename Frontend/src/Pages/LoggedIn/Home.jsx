import { useEffect, useState } from "react";
import ImageService from "../../Services/User/ImageService";
import { FaSpinner } from "react-icons/fa";
import UserImageCard from "../../Components/Cards/UserImageCard";

const Home = () => {
  const [images, setImages] = useState([]);
  useEffect(() => {
    const fetchData = async () => {
      try {
        const result = await ImageService.GetAllImages();
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
  if (images.length === 0) {
    return (
      <div className="w-full h-full flex items-center justify-center ml-0 lg:ml-20">
        <FaSpinner className="text-blue-500 text-4xl animate-spin" />
      </div>
    );
  }
  return (
    <div className="w-full ml-0 lg:ml-20">
      <div className="w-full">
        <div className="grid grid-cols-1 gap-5 md:grid-cols-2 lg:grid-cols-3">
          {images.map((image, index) => (
            <UserImageCard
              key={index}
              imageTitle={image.title}
              imageDescription={image.description}
              image={image.fileContentBase64}
              uploadDate={image.uploadDate}
              imageId={image.id}
              privatePage={false}
            />
          ))}
        </div>
      </div>
    </div>
  );
};

export default Home;
