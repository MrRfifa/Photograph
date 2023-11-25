import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { FaSpinner } from "react-icons/fa";
import ImageContent from "../../Components/ImageDetailsComponents/ImageContent";
import ImageComment from "../../Components/ImageDetailsComponents/ImageComment";

const ImageDetails = () => {
  const imageId = useParams();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      await new Promise((resolve) => setTimeout(resolve, 1000));
      setLoading(false);
    };
    if (imageId) {
      fetchData();
    }
  }, [imageId]);

  if (loading) {
    return (
      <div className="w-full h-full flex items-center justify-center">
        <FaSpinner className="text-blue-500 text-4xl animate-spin" />
      </div>
    );
  }

  return (
    <div className="md:ml-[10rem] w-full">
      <div className="grid grid-cols-1 lg:grid-cols-2 h-full gap-5">
        <ImageContent imageId={imageId.id} />
        <ImageComment imageId={imageId.id} />
      </div>
    </div>
  );
};

export default ImageDetails;
