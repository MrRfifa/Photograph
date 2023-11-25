import PropTypes from "prop-types";
import { useEffect, useState } from "react";
import CommentService from "../../Services/User/CommentService";
import AuthService from "../../Services/Auth/AuthService";
import profile_Image from "../../assets/profile-image.png";

const ImageComment = ({ imageId }) => {
  const [numberComments, setNumberComments] = useState(0);
  const [emptyCommentError, setEmptyCommentError] = useState(false);
  const [comments, setComments] = useState([]);
  const [commentText, setCommentText] = useState("");
  const [currentPage, setCurrentPage] = useState(1);
  const [commentsPerPage] = useState(5);

  // Move fetchComments outside of useEffect
  const fetchComments = async () => {
    try {
      const result = await CommentService.GetCommentsPerImageId(imageId);
      if (result.success) {
        const commentsWithUserInfo = await Promise.all(
          result.message.message.map(async (comment) => {
            try {
              const userInfoResponse = await AuthService.getUserSpecificInfo(
                comment.userId
              );

              if (userInfoResponse.userInfoSpec) {
                return {
                  ...comment,
                  userName: `${userInfoResponse.userInfoSpec.message.firstName} ${userInfoResponse.userInfoSpec.message.lastName}`,
                  profileImage:
                    userInfoResponse.userInfoSpec.message.fileContentBase64 ===
                    ""
                      ? profile_Image
                      : `data:image/png;base64,${userInfoResponse.userInfoSpec.message.fileContentBase64}`,
                };
              } else {
                console.error(
                  `Error fetching user information for userId ${comment.userId}`
                );
                return comment;
              }
            } catch (error) {
              console.error(
                `Error fetching user information for userId ${comment.userId}:`,
                error
              );
              return comment;
            }
          })
        );

        setComments(commentsWithUserInfo);
      } else {
        console.error("Error getting image comments:", result.error);
      }
    } catch (error) {
      console.error("Error getting image comments:", error);
    }
  };

  const handleComment = async () => {
    try {
      if (!commentText.trim()) {
        setEmptyCommentError(true);
        return;
      }

      const result = await CommentService.CommentImage(imageId, commentText);

      if (result.success) {
        // Fetch comments again to include the newly added comment
        await fetchComments();

        setCommentText("");
        setNumberComments((prevComments) => prevComments + 1);
      } else {
        console.error("Error commenting image:", result.error);
      }
    } catch (error) {
      console.error("Error commenting image:", error);
    }
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [numberCommentsResponse] = await Promise.all([
          CommentService.GetNumberCommentsPerImage(imageId),
        ]);

        setNumberComments(numberCommentsResponse.message.message);
      } catch (error) {
        console.error("Error fetching image details:", error);
      }
    };

    fetchComments();
    fetchData();
  }, [imageId]);

  // Pagination logic
  const indexOfLastComment = currentPage * commentsPerPage;
  const indexOfFirstComment = indexOfLastComment - commentsPerPage;
  const currentComments = comments.slice(
    indexOfFirstComment,
    indexOfLastComment
  );

  // Change page
  const paginate = (pageNumber) => setCurrentPage(pageNumber);

  // Customize the pagination styles
  const paginationButtonStyles =
    "px-4 py-2 mx-1 text-white rounded hover:bg-gray-600 focus:outline-none focus:shadow-outline";

  return (
    <div className="w-full h-full p-2 md:mt-20">
      <div className="w-full flex flex-col space-y-7">
        <div className="rounded-xl flex flex-col space-y-5">
          <textarea
            required
            className="rounded-xl bg-gray-700 p-2 text-white"
            value={commentText}
            onChange={(e) => {
              setCommentText(e.target.value);
              setEmptyCommentError(false); // Reset error when the user types
            }}
          />
          {emptyCommentError && (
            <p className="text-red-500">Comment cannot be empty.</p>
          )}
          <div className="flex flex-row justify-between">
            <button
              className="text-white p-2 rounded-full bg-[#144552] hover:bg-[#006466] w-[80%]"
              onClick={handleComment}
            >
              Comment
            </button>
            <p className="w-[15%] bg-[#22577a] rounded-full text-2xl text-center py-1 text-white ">
              {numberComments}📑
            </p>
          </div>
        </div>
        <div className="h-full flex flex-col">
          {currentComments.map((comment, index) => (
            <div key={index} className="comment-item mb-5">
              <div className="flex flex-row rounded-lg border-2 border-solid items-center text-white">
                <div className="p-2 mr-2 rounded-full flex flex-col justify-between items-center">
                  <img
                    src={comment.profileImage}
                    className="rounded-full w-12"
                  />
                  <p>{comment.userName}</p>
                </div>
                <p>{comment.text}</p>
              </div>
            </div>
          ))}
        </div>
        <div className="flex justify-center items-center mt-4">
          {Array.from({
            length: Math.ceil(comments.length / commentsPerPage),
          }).map((_, index) => (
            <button
              key={index}
              onClick={() => paginate(index + 1)}
              className={
                index + 1 === currentPage
                  ? `${paginationButtonStyles} bg-[#22577a]`
                  : paginationButtonStyles
              }
            >
              {index + 1}
            </button>
          ))}
        </div>
      </div>
    </div>
  );
};

ImageComment.propTypes = {
  imageId: PropTypes.string.isRequired,
};

export default ImageComment;
