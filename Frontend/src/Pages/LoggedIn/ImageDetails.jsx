import { useParams } from "react-router-dom";
import testImage from "../../assets/cam.png";
//
const ImageDetails = () => {
  const imageId = useParams();
  console.log(imageId);
  return (
    <div className="md:ml-[10rem] w-full">
      <div className="grid grid-cols-1 md:grid-cols-2 h-screen">
        <div className="bg-yellow-500 ">
          <div className="flex flex-col justify-normal h-full">
            <div className="rounded-xl border border-solid flex flex-col">
              <img src={testImage} alt="" />
            </div>
            <div className="flex flex-col justify-evenly h-full">
              <h1>title</h1>
              <p>
                Lorem ipsum dolor sit amet consectetur adipisicing elit. Ducimus
                iste vero natus repellendus odit distinctio earum vel, voluptas
                quo sequi veritatis impedit, maxime fugiat alias aperiam enim
                tenetur.
              </p>
              <button className="bg-blue-900 w-full p-2 rounded-full ">
                like
              </button>
              <h1>©Hamed kozdoghli</h1>
            </div>
          </div>
        </div>
        <div className="bg-red-500 w-full h-full">hello</div>
      </div>
    </div>
  );
};

export default ImageDetails;
