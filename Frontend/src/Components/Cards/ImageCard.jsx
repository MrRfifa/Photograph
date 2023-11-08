import PropTypes from "prop-types";

const ImageCard = ({ image }) => {
  return (
    <div className="bg-white h-64 w-full shadow-xl shadow-black flex flex-col p-4 my-4 rounded-lg hover:scale-105 duration-300">
      <div className="flex p-2 gap-1">
        <div>
          <span className="bg-blue-500 inline-block w-3 h-3 rounded-full"></span>
        </div>
        <div className="circle">
          <span className="bg-purple-500 inline-block w-3 h-3 rounded-full"></span>
        </div>
        <div className="circle">
          <span className="bg-pink-500 box inline-block w-3 h-3 rounded-full"></span>
        </div>
      </div>
      <div>
        <div className="rounded-full overflow-hidden border border-white p-1">
          <img src={image} alt={image} className="w-full h-[75%]" />
        </div>
      </div>
    </div>
  );
};

export default ImageCard;

ImageCard.propTypes = {
  image: PropTypes.string.isRequired,
};
