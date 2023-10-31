import ImageCard from "../Components/ImageCard";
import dark_1 from "../assets/Cards/dark-1.jpg";
import dark_2 from "../assets/Cards/dark-2.jpg";
import dark_3 from "../assets/Cards/dark-3.jpg";
import light_1 from "../assets/Cards/light-1.jpg";
import light_2 from "../assets/Cards/light-2.jpg";
import light_3 from "../assets/Cards/light-3.jpg";

const CardsSection = () => {
  return (
    <>
      <div className="w-full py-[10rem] px-4">
        <div className="max-w-[1240px] mx-auto grid md:grid-cols-3 gap-8">
          <ImageCard image={light_1} />
          <ImageCard image={light_2} />
          <ImageCard image={light_3} />
        </div>
      </div>
      <div className="w-full py-[10rem] px-4 bg-white">
        <div className="max-w-[1240px] mx-auto grid md:grid-cols-3 gap-8">
          <ImageCard image={dark_1} />
          <ImageCard image={dark_2} />
          <ImageCard image={dark_3} />
        </div>
      </div>
    </>
  );
};

export default CardsSection;
